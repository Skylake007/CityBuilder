using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : BinBeha
{
	public static BuildingManager instance;
    [SerializeField] protected List<BuildingCtrl> buildingCtrls;

	protected override void Awake()
	{
		base.Awake();
		if (BuildingManager.instance != null) Debug.LogError("Only 1 BuildingManager allow exist");
		BuildingManager.instance = this;
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadBuildingCtrls();
	}

	protected virtual void LoadBuildingCtrls()
	{
		if (this.buildingCtrls.Count > 0) return;
		foreach (Transform chil in transform)
		{
			BuildingCtrl ctrl = chil.GetComponent<BuildingCtrl>();
			if (ctrl == null) continue;
			this.buildingCtrls.Add(ctrl);
		}

		Debug.Log(transform.name + ": LoadBuildingCtrls", gameObject);
	}

	public virtual BuildingCtrl FindBuilding(BuildingType buildingType)
	{
		BuildingCtrl buildingCtrl;
		for (int i = 0; i < this.buildingCtrls.Count; i++)
		{
			buildingCtrl = this.buildingCtrls[i];
			if (!buildingCtrl.workers.IsNeedWorker()) continue;
			if (buildingCtrl.buildingType != buildingType) continue;

			return buildingCtrl;
		}
		return null;
	}

	public virtual List<BuildingCtrl> BuildingCtrls()
	{
		return this.buildingCtrls;
	}

	public virtual void AddBuilding(BuildingCtrl buildingCtrl)
	{
		Debug.Log("BuildingCtrl" + buildingCtrl);

		this.buildingCtrls.Add(buildingCtrl);
		buildingCtrl.transform.parent = transform;
	}
}
