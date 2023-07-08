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
		if (this.currentBuild == null) return;

		Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
		//Diem cham giua con chuot va mat phang

		int mask = (1 << MyLayerManager.instance.layerGround);
		if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
		{
			this.buildPos = hit.point;
			this.currentBuild.position = this.buildPos;
		}
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
