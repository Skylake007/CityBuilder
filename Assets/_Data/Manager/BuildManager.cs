using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : BinBeha
{
	public static BuildManager instance;
	public bool isBuilding = false;
	[SerializeField] protected Vector3 buildPos;
	[SerializeField] protected Transform currentBuild;
	[SerializeField] protected List<Transform> buildPrefabs;

	private BuildingCtrl buildingCtrl;
	private WorkerCtrl workerCtrl;
	private ConstructionCtrl constructionCtrl;
	private TreeCtrl treeCtrl;

	[SerializeField] protected float timer = 0f;
	[SerializeField] protected float timerClear = 5;

	protected override void Awake()
	{
		base.Awake();
		if (BuildManager.instance != null) Debug.LogError("Only 1 BuildManager allow exist");
		BuildManager.instance = this;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.ChoosePlaceToBuild();
		this.UpdateWorkerIns();
		this.UpdateConstructionIns();
		this.UpdateBuildingIns();
		this.UpdateTreeIns();

		this.timer += Time.fixedDeltaTime;
		if (this.timer < this.timerClear) return;
		this.ClearVar();
		this.timer = 0;

	}

	protected override void Start()
	{
		base.Start();
		this.HideAllPrefabs();
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadBuildPrefabs();
	}

	protected virtual void LoadBuildPrefabs()
	{
		if (this.buildPrefabs.Count > 0) return;
		foreach (Transform child in transform)
		{
			this.buildPrefabs.Add(child);
		}

		Debug.Log(transform.name + ": LoadBuildPrefabs", gameObject);
	}

	protected virtual void HideAllPrefabs()
	{
		foreach (Transform build in this.buildPrefabs)
		{
			build.gameObject.SetActive(false);
		}
	}

	public virtual void CurrentBuildSet(string buildName)
	{
		this.isBuilding = false;
		if (this.currentBuild != null) this.currentBuild.gameObject.SetActive(false);

		foreach (Transform build in this.buildPrefabs)
		{	
			if (build.name != buildName) continue;

			this.currentBuild = build;
			this.currentBuild.gameObject.SetActive(true);
			Invoke("SetIsBuilding", 0.5f);
		}
	}

	protected virtual void SetIsBuilding()
	{
		this.isBuilding = true;
	}

	public virtual void CurrentBuildClear()
	{
		if (currentBuild == null) return;
		this.currentBuild.gameObject.SetActive(false);
		this.currentBuild = null;
	}

	protected virtual void ChoosePlaceToBuild()
	{
		Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
		int maskBuilding = (1 << MyLayerManager.instance.layerBuilding);
		int maskGround = (1 << MyLayerManager.instance.layerGround);
		int maskWorker = (1 << MyLayerManager.instance.layerWorker);
		int maskTree = (1 << MyLayerManager.instance.layerTree);


		//TODO: Make is shorter

		if (Physics.Raycast(ray, out RaycastHit hitBuilding, 999, maskBuilding) && Input.GetMouseButtonDown(0))
		{
			GameObject gameObject = hitBuilding.collider.gameObject;
			Debug.Log("Raycast building" + gameObject.name);
			Component[] components = gameObject.GetComponents<Component>();

			foreach (Component component in components)
			{
				BuildingCtrl buildingCtrl = component.GetComponent<BuildingCtrl>();
				ConstructionCtrl constructionCtrl = component.GetComponent<ConstructionCtrl>();

				if (buildingCtrl != null)
				{
					this.timer = 0f;
					this.workerCtrl = null;
					this.constructionCtrl = null;
					this.buildingCtrl = buildingCtrl;
					this.treeCtrl = null;
				}

				if (constructionCtrl != null)
				{
					this.timer = 0f;
					this.workerCtrl = null;
					this.constructionCtrl = constructionCtrl;
					this.buildingCtrl = null;
					this.treeCtrl = null;
				}
			}
			return;
		}
		else if (Physics.Raycast(ray, out RaycastHit hitWorker, 999, maskWorker) && Input.GetMouseButtonDown(0))
		{
			GameObject gameObject = hitWorker.collider.gameObject;
			Debug.Log("Raycast worker" + gameObject.name);
			Component[] components = gameObject.GetComponents<Component>();

			foreach (Component component in components)
			{
				WorkerCtrl workerCtrl = component.GetComponent<WorkerCtrl>();
				if (workerCtrl != null)
				{
					this.timer = 0f;
					this.workerCtrl = workerCtrl;
					this.constructionCtrl = null;
					this.buildingCtrl = null;
					this.treeCtrl = null;
				}
			}
			return;
		}
		else if (Physics.Raycast(ray, out RaycastHit hitTree, 999, maskTree) && Input.GetMouseButtonDown(0))
		{
			GameObject gameObject = hitTree.collider.gameObject;
			Debug.Log("Raycast worker" + gameObject.name);
			Component[] components = gameObject.GetComponents<TreeCtrl>();

			foreach (Component component in components)
			{
				TreeCtrl treeCtrl = component.GetComponent<TreeCtrl>();
				if (treeCtrl != null)
				{
					this.timer = 0f;
					this.workerCtrl = null;
					this.constructionCtrl = null;
					this.buildingCtrl = null;
					this.treeCtrl = treeCtrl;
				}
			}
			return;
		}

		else if (Physics.Raycast(ray, out RaycastHit hitGround, 999, maskGround) && this.currentBuild != null)
		{
			this.buildPos = hitGround.point;
			this.currentBuild.position = this.buildPos;
			return;
		}
	}

	private void UpdateWorkerIns()
	{
		if (this.workerCtrl == null) return;
		UIPopupInspectorManager.Instance.ShowItems(workerCtrl);
	}

	private void UpdateConstructionIns()
	{
		if (this.constructionCtrl == null) return;
		UIPopupInspectorManager.Instance.ShowItems(constructionCtrl);
	}

	private void UpdateBuildingIns()
	{
		if (this.buildingCtrl == null) return;
		UIPopupInspectorManager.Instance.ShowItems(buildingCtrl);
	}

	private void UpdateTreeIns()
	{
		if (this.treeCtrl == null) return;
		UIPopupInspectorManager.Instance.ShowItems(treeCtrl);
	}

	protected virtual void ClearVar()
	{
		this.workerCtrl = null;
		this.constructionCtrl = null;
		this.buildingCtrl = null;
		this.treeCtrl = null;
		UIPopupInspectorManager.Instance.Close();
	}

	public virtual void CurrentBuildPlace()
	{
		if (this.currentBuild == null) return;

		ConstructionCtrl constructionCtrl = this.currentBuild.GetComponent<ConstructionCtrl>();
		if (constructionCtrl && constructionCtrl.limitRadius.IsCollided())
		{
			Debug.LogWarning("Collided: " + constructionCtrl.limitRadius.colliderObjs);
			return;
		}

		string constructionName = constructionCtrl.abstractConstruction.GetConstructionName();
		if (constructionName != null)
		{
			Transform newBuild = PrefabManager.instance.Instantiate(constructionName);
			newBuild.position = this.buildPos;
			newBuild.name = this.currentBuild.name;
			newBuild.gameObject.SetActive(true);

			AbstractConstruction abs = newBuild.GetComponent<AbstractConstruction>();
			abs.isPlaced = true;
			ConstructionManager.instance.AddConstruction(abs);
		}

		this.currentBuild.gameObject.SetActive(false);
		this.currentBuild = null;
		this.isBuilding = false;
	}

	private void OnDrawGizmosSelected()
	{
		if (this.currentBuild == null) return;
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(GodModeCtrl.Instance._camera.transform.position, this.buildPos);
		}
	}
}
