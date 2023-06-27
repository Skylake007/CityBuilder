using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawmillTask : BuildingTask
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
			case TaskType.makingResource:
				this.MakingResource(workerCtrl);
				break;
			case TaskType.gotoWorkPoint:
			this.GotoWorkingPoint(workerCtrl);
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
		if (!this.IsStoreMax() && this.HasLogwood())
		{
			workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
			workerCtrl.workerTasks.TaskAdd(TaskType.makingResource);
			workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkPoint);
		}
	}

	protected virtual bool IsStoreMax()
	{ return false; }

	protected virtual bool HasLogwood()
	{ return true; }
}
