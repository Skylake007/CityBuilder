using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCtrl : BinBeha
{
	public Transform door;
	public Warehouse warehouse;
	public Workers workers;
	public BuildingTask buildingTask;

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadWorkers();
		this.LoadDoor();
		this.LoadWarehouse();
		this.LoadBuildingTask();
	}
	protected virtual void LoadWarehouse()
	{
		if (this.warehouse != null) return;
		this.warehouse = GetComponent<Warehouse>();
		Debug.Log(transform.name + " LoadWarehouse", gameObject);
	}

	protected virtual void LoadWorkers()
	{
		if (this.workers != null) return;
		this.workers = GetComponent<Workers>();
		Debug.LogWarning(transform.name + ": LoadWorker", gameObject);
	}

	protected virtual void LoadDoor()
	{
		if (this.door != null) return;
		this.door = transform.Find("Door");
		Debug.LogWarning(transform.name + ": LoadDoor", gameObject);
	}

	protected virtual void LoadBuildingTask()
	{
		if (this.buildingTask != null) return;
		this.buildingTask = GetComponent<BuildingTask>();
		Debug.LogWarning(transform.name + ": LoadBuildingTask", gameObject);
	}
}
