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
		this.currentBuild = null;
	}

	protected virtual void ChoosePlaceToBuild()
	{
		if (this.currentBuild == null) return;
		Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);
		//Diem cham giua con chuot va mat phang

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			this.buildPos = hit.point;
			this.currentBuild.position = this.buildPos;
		}
	}

	public virtual void CurrentBuildPlace()
	{
		GameObject newBuild = Instantiate(this.currentBuild.gameObject);
		newBuild.transform.position = this.buildPos;

		this.currentBuild.gameObject.SetActive(false);
		this.currentBuild = null;
		this.isBuilding = false;

		AbstractConstruction abstractConstruction = newBuild.GetComponent<AbstractConstruction>();
		ConstructionManager.instance.AddConstruction(abstractConstruction);

	}

	private void OnDrawGizmosSelected()
	{
		if (this.currentBuild == null) return;
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(GodModeCtrl.instance._camera.transform.position, this.buildPos);
		}
	}
}
