using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestroyBuilding : BuildDestroyable
{
	public BuildingCtrl buildingCtrl;

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadBuildingCtrl();
	}

	protected virtual void LoadBuildingCtrl()
	{
		if (this.buildingCtrl != null) return;
		this.buildingCtrl = GetComponent<BuildingCtrl>();
		Debug.Log(transform.name + ": LoadBuildingCtrl", gameObject);
	}

	public override void Destroy()
	{
		this.buildingCtrl.workers.ReleaseWorkers();
		BuildingManager.instance.RemoveBuilding(this.buildingCtrl);
		base.Destroy();
	}
}
