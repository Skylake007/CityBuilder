using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTask : BuildingTask
{
	[Header("Sawmill Task")]
	[SerializeField] protected Transform workingPoint;
	//[SerializeField] protected float workingSpeed = 7;


	protected override void LoadComponents()
	{
		base.LoadComponents();
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
				Debug.Log("makingResource ");
				break;
			default:
				if (this.IsTimeToWork()) this.Planning(workerCtrl);
				break;
		}
	}

	protected virtual void Planning(WorkerCtrl workerCtrl)
	{


	}
}
