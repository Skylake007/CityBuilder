using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseTask : BuildingTask
{
	[Header("Sawmill Task")]
	[SerializeField] protected Transform workingPoint;
	[SerializeField] protected int logwoodCount = 0;
	[SerializeField] protected float blankReceive = 0f;
	//[SerializeField] protected float workingSpeed = 7;


	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadWorkingPoint();
	}

	protected virtual void LoadWorkingPoint()
	{
		if (this.workingPoint != null) return;
		this.workingPoint = transform.Find("WorkingPoint");
		Debug.Log(transform.name + " LoadObject", gameObject);
	}

	public override void DoingTask(WorkerCtrl workerCtrl)
	{
		switch (workerCtrl.workerTasks.TaskCurrent())
		{
			case TaskType.getResNeedToMove:
				this.GoGetResNeedToMove(workerCtrl);
				break;
			case TaskType.bringResourceBack:
			this.BringResourceBack(workerCtrl);
				break;
			case TaskType.goToWorkStation:
				this.BackToWorkStation(workerCtrl);
				break;
			default:
				if (this.IsTimeToWork()) this.Planning(workerCtrl);
				break;
		}
	}

	protected virtual void GotoWorkingPoint(WorkerCtrl workerCtrl)
	{
		WorkerTasks workerTasks = workerCtrl.workerTasks;
		if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();
	}

	protected virtual void MakingResource(WorkerCtrl workerCtrl)
	{
		if (workerCtrl.workerMovement.isWorking) return;
		StartCoroutine(Sawing(workerCtrl));
	}

	IEnumerator Sawing(WorkerCtrl workerCtrl)
	{
		workerCtrl.workerMovement.isWorking = true;
		workerCtrl.workerMovement.workingType = WorkingType.sawing;
		yield return new WaitForSeconds(this.workingSpeed);

		this.buildingCtrl.warehouse.RemoveResource(ResourceName.logwood, this.logwoodCount);
		this.buildingCtrl.warehouse.AddResource(ResourceName.blank, this.blankReceive);

		workerCtrl.workerMovement.isWorking = false;
		workerCtrl.workerTasks.TaskCurrentDone();
	}

	protected virtual void Planning(WorkerCtrl workerCtrl)
	{
		BuildingCtrl buildingCtrl = this.GetWorkStationHasResNeedToMove();
		if (buildingCtrl != null) {
			workerCtrl.workerTasks.taskBuildingCtrl = buildingCtrl;
			workerCtrl.workerMovement.SetTarget(null);
			workerCtrl.workerTasks.TaskAdd(TaskType.getResNeedToMove);
		}
	}

	protected virtual void GoGetResNeedToMove(WorkerCtrl workerCtrl)
	{
		WorkerTasks workerTasks = workerCtrl.workerTasks;
		if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

		BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
		ResHolder resHolder = taskBuildingCtrl.warehouse.ResNeedToMove();

		if (resHolder == null)
		{
			this.DoneGetResNeedToMove(workerCtrl);
			return;
		}
		if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
		if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

		float count = 1;
		resHolder.Deduct(count);
		workerCtrl.resCarrier.AddResource(resHolder.Name(), count);
		this.DoneGetResNeedToMove(workerCtrl);

		Resource res = workerCtrl.resCarrier.Resources()[0];
		BuildingCtrl buildingCtrl = this.FindBuildingNeedRes(res);
		workerTasks.taskBuildingCtrl = buildingCtrl;
		workerTasks.TaskAdd(TaskType.bringResourceBack);
	}

	protected virtual void DoneGetResNeedToMove(WorkerCtrl workerCtrl)
	{
		workerCtrl.workerTasks.TaskCurrentDone();
		workerCtrl.workerTasks.taskBuildingCtrl = null;
		workerCtrl.workerMovement.SetTarget(null);
	}

	protected virtual void BringResourceBack(WorkerCtrl workerCtrl)
	{
		WorkerTasks workerTasks = workerCtrl.workerTasks;
		if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

		BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
		if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);

		if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

		workerCtrl.workerMovement.SetTarget(null);
		workerTasks.taskBuildingCtrl = null;
		workerTasks.TaskCurrentDone();

		Resource res = workerCtrl.resCarrier.TakeFirst();
		taskBuildingCtrl.warehouse.AddResource(res.name, res.number);

		ResHolder resHolder = taskBuildingCtrl.warehouse.ResNeedToMove();
		if (resHolder == null) return;
		workerTasks.taskBuildingCtrl = taskBuildingCtrl;
		workerTasks.TaskAdd(TaskType.getResNeedToMove);
	}

	protected virtual BuildingCtrl GetWorkStationHasResNeedToMove()
	{
		foreach (BuildingCtrl buildingCtrl in BuildingManager.instance.BuildingCtrls())
		{
			if (buildingCtrl.warehouse.buildingType != BuildingType.workStation) continue;
			ResHolder resHolder = buildingCtrl.warehouse.ResNeedToMove();
			if (resHolder == null) continue;
			return buildingCtrl;
		}
		return null;
	}

	protected virtual BuildingCtrl FindBuildingNeedRes(Resource res)
	{
		foreach (BuildingCtrl buildingCtrl in BuildingManager.instance.BuildingCtrls())
		{
			if (buildingCtrl.warehouse.buildingType != BuildingType.workStation) continue;
			ResHolder resHolder = buildingCtrl.warehouse.IsNeedRes(res);
			if (resHolder == null) continue;
			return buildingCtrl;
		}
		return this.buildingCtrl;
	}

	protected virtual bool IsStoreMax()
	{ return false; }

	protected virtual bool HasLogwood()
	{ return true; }
}
