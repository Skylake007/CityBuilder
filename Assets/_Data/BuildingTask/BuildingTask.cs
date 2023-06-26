using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : BinBeha
{
	[Header("Building Task")]
	public BuildingCtrl buildingCtrl;
	[SerializeField] protected float taskTimer = 0f;
	[SerializeField] protected float timeDelay = 5f;
	[SerializeField] protected float workingSpeed = 7;


	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadBuildingCtrl();
	}

	protected virtual void LoadBuildingCtrl()
	{
		if (this.buildingCtrl != null) return;
		this.buildingCtrl = GetComponent<BuildingCtrl>();
		Debug.Log(transform.name + " LoadBuildingCtrl", gameObject);
	}

	protected virtual bool IsTimeToWork()
	{
		this.taskTimer += Time.fixedDeltaTime;
		if (this.taskTimer < this.timeDelay) return false;
		this.taskTimer = 0;
		return true;
	}

	protected virtual void BackToWorkStation(WorkerCtrl workerCtrl)
	{
		WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
		taskWorking.GotoBuilding();
		if (workerCtrl.workerMovement.IsCloseToTarget())
		{
			taskWorking.GoIntoBuilding();
			workerCtrl.workerTasks.TaskCurrentDone();
		}
	}

	public virtual void DoingTask(WorkerCtrl workerCtrl)
	{
		//For override
	}

	public virtual void FindNearByBuildings()
	{
		//For override}
	}
}
