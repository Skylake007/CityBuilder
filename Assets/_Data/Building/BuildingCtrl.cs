using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCtrl : BinBeha
{
	[Header("Building")]
	public BuildingType buildingType = BuildingType.workStation;
	public Transform door;
	public Workers workers;
	public Warehouse warehouse;
	public BuildingTask buildingTask;

	private Image image;
	[SerializeField] protected List<WorkerCtrl> workerCtrls;
	[SerializeField] protected List<ResHolder> resHolders;

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadWorkers();
		this.LoadDoor();
		this.LoadWarehouse();
		this.LoadBuildingTask();
		this.LoadImgIns();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.LoadImgIns();
		this.LoadWorkerIns();
		this.LoadResIns();
	}

	protected virtual void LoadWorkerIns()
	{
		List<WorkerCtrl> workers = this.workers.GetCurrentWorker();
		if (this.workerCtrls.Count > 0) return;
		foreach(WorkerCtrl workerCtrl in workers)
		{
			this.workerCtrls.Add(workerCtrl);
		}
	}

	protected virtual void LoadResIns()
	{
		List<ResHolder> resHolders = this.warehouse.GetResStorage();
		if (this.resHolders.Count > 0) return;
		foreach (ResHolder resHolder in resHolders)
		{
			this.resHolders.Add(resHolder);
		}
	}

	protected virtual void LoadImgIns()
	{
		if (this.image != null) return;
		this.image = GetComponent<Image>();
		Debug.Log(transform.name + ": LoadImage", gameObject);
	}

	protected virtual void LoadWorkers()
	{
		if (this.workers != null) return;
		this.workers = GetComponent<Workers>();
		Debug.Log(transform.name + ": LoadWorker", gameObject);
	}

	protected virtual void LoadDoor()
	{
		if (this.door != null) return;
		this.door = transform.Find("Door");
		Debug.Log(transform.name + ": LoadDoor", gameObject);
	}

	protected virtual void LoadWarehouse()
	{
		if (this.warehouse != null) return;
		this.warehouse = GetComponent<Warehouse>();
		Debug.Log(transform.name + " LoadWarehouse", gameObject);
	}

	protected virtual void LoadBuildingTask()
	{
		if (this.buildingTask != null) return;
		this.buildingTask = GetComponent<BuildingTask>();
		Debug.Log(transform.name + ": LoadBuildingTask", gameObject);
	}

	public virtual List<WorkerCtrl> GetWorkerCtrl()
	{
		return this.workerCtrls;
	}

	public virtual List<ResHolder> GetResHolders()
	{
		return this.resHolders;
	}

	public virtual Image GetImgIns()
	{
		return this.image;
	}
}
