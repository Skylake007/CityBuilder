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

		Debug.LogWarning(transform.name + ": LoadBuildingCtrls", gameObject);
	}

	public virtual BuildingCtrl FindBuilding(Transform worker, BuildingType buildingType)
	{
	//To do
		BuildingCtrl buildingCtrl;
		for (int i = 0; i < this.buildingCtrls.Count; i++)
		{
			buildingCtrl = this.buildingCtrls[i];
			if (!buildingCtrl.workers.IsNeedWorker()) continue;
			if (buildingCtrl.warehouse.buildingType != buildingType) continue;

			buildingCtrl.workers.AddWorker(worker);
			return buildingCtrl;
		}
		return null;
	}
}
