using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHutTask : BuildingTask
{
	[Header("Forest Hut Task")]
	[SerializeField] protected GameObject plantTreeObj;
	[SerializeField] protected int treeMax = 7;
	[SerializeField] protected float treeRange = 27f;
	[SerializeField] protected float treeDistance = 7f;
	[SerializeField] protected float treeRemoveSpeed = 16;
	[SerializeField] protected List<GameObject> treePrefabs;
	[SerializeField] protected List<GameObject> trees;

	protected override void Start()
	{
		base.Start();
		this.LoadNearByTrees();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.RemoveDeadTrees();
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadObjects();
		this.LoadTreePrefabs();
	}

	protected virtual void LoadObjects()
	{
		if (this.plantTreeObj != null) return;
		this.plantTreeObj = Resources.Load<GameObject>("Building/MaskPositionObject");
		Debug.LogWarning(transform.name + " LoadObject", gameObject);
	}

	protected virtual void LoadTreePrefabs()
	{
		if (this.treePrefabs.Count > 0) return;
		GameObject tree1 = Resources.Load<GameObject>("Res/Tree_1");
		GameObject tree2 = Resources.Load<GameObject>("Res/Tree_2");
		GameObject tree3 = Resources.Load<GameObject>("Res/Tree_3");

		this.treePrefabs.Add(tree1);
		this.treePrefabs.Add(tree2);
		this.treePrefabs.Add(tree3);
		Debug.LogWarning(transform.name + " LoadObjects", gameObject);
	}

	protected virtual void RemoveDeadTrees()
	{
		GameObject tree;
		for (int i = 0; i < this.trees.Count; i++)
		{
			tree = this.trees[i];
			if (tree == null) this.trees.RemoveAt(i);
		}
	}

	public override void DoingTask(WorkerCtrl workerCtrl)
	{
		switch (workerCtrl.workerTasks.TaskCurrent())
		{
			case TaskType.plantTree:
				this.PlantTree(workerCtrl);
				break;
			case TaskType.findTreeToChop:
				this.FindTreeToChop(workerCtrl);
				break;
			case TaskType.chopTree:
				this.ChopTree(workerCtrl);
				break;
			case TaskType.bringResourceBack:
				this.BringTreeBack(workerCtrl);
				break;
			case TaskType.goToWorkStation:
				this.BackToWorkStation(workerCtrl);
				break;
			default:
				if (this.IsTimeToWork()) this.Planning(workerCtrl);
				break;
		}
	}
	protected virtual void Planning(WorkerCtrl workerCtrl)
	{
		if (!this.buildingCtrl.warehouse.IsFull())
		{
			workerCtrl.workerTasks.TaskAdd(TaskType.bringResourceBack);
			workerCtrl.workerTasks.TaskAdd(TaskType.chopTree);
			workerCtrl.workerTasks.TaskAdd(TaskType.findTreeToChop);
		}

		if (this.NeedMoreTree())
		{
			workerCtrl.workerMovement.SetTarget(null);
			workerCtrl.workerTasks.TaskAdd(TaskType.plantTree);
		}
	}

	protected virtual bool NeedMoreTree()
	{
		return this.treeMax >= this.trees.Count;
	}

	protected virtual void PlantTree(WorkerCtrl workerCtrl)
	{
		Transform target = workerCtrl.workerMovement.GetTarget();

		if (target == null) target = this.GetPlantPlace();
		if (target == null) return;

		workerCtrl.workerTasks.taskWorking.GoOutBuilding();
		workerCtrl.workerMovement.SetTarget(target);

		if (workerCtrl.workerMovement.IsCloseToTarget())
		{
			workerCtrl.workerMovement.SetTarget(null);
			Destroy(target.gameObject); //to do
			this.Planting(workerCtrl.transform);

			if (!this.NeedMoreTree())
			{
				workerCtrl.workerTasks.TaskCurrentDone();
				workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
			}
		}
	}

	protected virtual void Planting(Transform trans)
	{
		GameObject treePrefab = this.GetTreePrefab();
		GameObject treeObj = Instantiate<GameObject>(treePrefab);
		treeObj.transform.position = trans.position;
		treeObj.transform.rotation = trans.rotation;
		this.trees.Add(treeObj);
		TreeManager.instance.TreeAdd(treeObj);
	}

	protected virtual GameObject GetTreePrefab()
	{
		int rand = Random.Range(0, this.treePrefabs.Count);
		return this.treePrefabs[rand];
	}

	protected virtual Transform GetPlantPlace()
	{
		Vector3 newTreePos = this.RandomPlaceForTree();
		float dis = Vector3.Distance(transform.position, newTreePos);
		if (dis < this.treeDistance)
		{
			Debug.Log("GetPlantPlace Destroy GameObject");
			return null;
		}

		GameObject treePlace = Instantiate(this.plantTreeObj);
		treePlace.transform.position = newTreePos;

		return treePlace.transform;
	}

	protected virtual Vector3 RandomPlaceForTree()
	{
		Vector3 pos = transform.position;
		pos.x += Random.Range(this.treeRange * -1, this.treeRange);
		pos.y = 0;
		pos.z += Random.Range(this.treeRange * -1, this.treeRange);

		return pos;
	}

	protected virtual void LoadNearByTrees()
	{
		List<GameObject> allTrees = TreeManager.instance.Trees();
		float dis;
		foreach (GameObject tree in allTrees)
		{
			dis = Vector3.Distance(tree.transform.position, transform.position);
			//Debug.Log(tree.name + ": " + dis);
			if (dis > this.treeRange) continue;
			this.TreeAdd(tree);
		}
	}

	public virtual void TreeAdd(GameObject tree)
	{
		if (this.trees.Contains(tree)) return;
		this.trees.Add(tree);
	}

	protected virtual void ChopTree(WorkerCtrl workerCtrl)
	{
		if (workerCtrl.workerMovement.isWorking) return;

		workerCtrl.workerMovement.isWorking = true;
		StartCoroutine(Chopping(workerCtrl, workerCtrl.workerTasks.taskTarget));
		Debug.Log("ChopTree");
	}

	private IEnumerator Chopping(WorkerCtrl workerCtrl, Transform tree)
	{
		workerCtrl.workerMovement.isWorking = true;
		yield return new WaitForSeconds(this.workingSpeed);

		TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>();
		treeCtrl.treeLevel.ShowLastBuild();
		List<Resource> resources = treeCtrl.logwoodGenerator.TakeAll();
		treeCtrl.choper = null;
		this.trees.Remove(treeCtrl.gameObject);
		TreeManager.instance.Trees().Remove(treeCtrl.gameObject);

		workerCtrl.workerMovement.isWorking = false;
		workerCtrl.workerTasks.taskTarget = null;
		workerCtrl.resCarrier.AddByList(resources);

		workerCtrl.workerTasks.TaskCurrentDone();

		StartCoroutine(RemoveTree(tree));
	}

	private IEnumerator RemoveTree(Transform tree)
	{
		yield return new WaitForSeconds(this.treeRemoveSpeed);
		Destroy(tree.gameObject);
	}
		
	protected virtual TreeCtrl GetNearestTree()
	{
		foreach (GameObject tree in this.trees)
		{
			TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>();
			if (treeCtrl.treeLevel.IsMaxLevel()) return treeCtrl;
		}

		return null;
	}

	protected virtual void FindTreeToChop(WorkerCtrl workerCtrl)
	{
		WorkerTasks workerTasks = workerCtrl.workerTasks;
		if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

		if (workerCtrl.workerTasks.taskTarget == null)
		{
			this.FindNearestTree(workerCtrl);
		}
		else if (workerCtrl.workerMovement.TargetDistance() <= 1.5f) //cach target 1 xiu~  
		{
			workerCtrl.workerMovement.SetTarget(null);
			workerCtrl.workerTasks.TaskCurrentDone();
		}
	}

	protected virtual void FindNearestTree(WorkerCtrl workerCtrl)
	{
		foreach (GameObject tree in this.trees)
		{
			TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>(); //to do can make it faster
			if (treeCtrl == null) continue;
			//if (!treeCtrl.treeLevel.IsAllResMax()) continue;
			if (treeCtrl.choper != null) continue;

			treeCtrl.choper = workerCtrl;
			workerCtrl.workerTasks.taskTarget = treeCtrl.transform;
			workerCtrl.workerMovement.SetTarget(treeCtrl.transform);
			return; 
		}
	}

	protected virtual void BringTreeBack(WorkerCtrl workerCtrl)
	{
		WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
		taskWorking.GotoBuilding();
		if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

		List<Resource> resources = workerCtrl.resCarrier.TakeAll();
		this.buildingCtrl.warehouse.AddByList(resources);
		taskWorking.GoIntoBuilding();

		workerCtrl.workerTasks.TaskCurrentDone();
	
	}
}
