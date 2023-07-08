using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseTask : BuildingTask
{
    [Header("Warehouse")]
    [SerializeField] protected int takeProductCount = 0;
    [SerializeField] protected int takeProductMax = 7;
    [SerializeField] protected float takeProductTimer = 0;
    [SerializeField] protected float takeProductDelay = 7f;

    [SerializeField] protected int bringMaterialCount = 0;
    [SerializeField] protected int bringMaterialMax = 2;
    [SerializeField] protected float bringMaterialTimer = 0;
    [SerializeField] protected float bringMaterialDelay = 7f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    /// <summary>
    /// Called in FixedUpdate
    /// </summary>
    /// <param name="workerCtrl"></param>

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findBuildingHasProduct:
                this.FindBuildingHasProduct(workerCtrl);
                break;
            case TaskType.goToGetProduct:
                this.GoToGetProduct(workerCtrl);
                break;
            case TaskType.takingProductBack:
                this.BringResourceBack(workerCtrl);
                break;
            case TaskType.findBuildingNeedMaterial:
                this.FindBuildingNeedMaterial(workerCtrl);
                break;
            case TaskType.bringMaterialToBuilding:
                this.BringMaterialToBuilding(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.GoToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTimeToWork()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingNeedMaterial);
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingHasProduct);

        this.bringMaterialCount = this.bringMaterialMax;
        this.takeProductCount = this.takeProductMax;
    }

    protected virtual void FindBuildingHasProduct(WorkerCtrl workerCtrl)
    {
        this.takeProductTimer += Time.fixedDeltaTime;
        if (this.takeProductTimer > this.takeProductDelay)
        {
            this.takeProductCount--;
            this.takeProductTimer = 0;
        }

        if (this.takeProductCount < 0)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            return;
        }

        BuildingCtrl buildingCtrl = this.FindBuildingHasProductOld(workerCtrl);
        if (buildingCtrl != null)
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.goToGetProduct);
            this.takeProductTimer = 0;
            this.takeProductCount--;
        }
    }

    protected virtual void FindBuildingNeedMaterial(WorkerCtrl workerCtrl)
    {
        this.bringMaterialTimer += Time.fixedDeltaTime;
        if (this.bringMaterialTimer > this.bringMaterialDelay)
        {
            this.bringMaterialCount--;
            this.bringMaterialTimer = 0;
        }

        if (this.bringMaterialCount < 0)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            return;
        }

        List<Resource> resources;
        ResHolder resHolder;
        int carryCount = workerCtrl.resCarrier.carryCount;

        foreach (BuildingCtrl buildingCtrl in this.nearBuildings)
        {
            if (buildingCtrl.buildingType != BuildingType.workStation) continue;
            resources = buildingCtrl.warehouse.NeedResources();
            foreach (Resource resource in resources)
            {
                resHolder = this.buildingCtrl.warehouse.GetResource(resource.name);
                if (resHolder.Current() < 1) continue;

                this.buildingCtrl.warehouse.RemoveResource(resource.name, carryCount);
                workerCtrl.resCarrier.AddResource(resource.name, carryCount);

                workerCtrl.workerTasks.taskBuildingCtrl = buildingCtrl;
                workerCtrl.workerTasks.TaskAdd(TaskType.bringMaterialToBuilding);

                this.bringMaterialCount--;
                this.bringMaterialTimer = 0;
                return;
            }
        }
    }

    protected virtual void GoToGetProduct(WorkerCtrl workerCtrl)
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

        float count = workerCtrl.resCarrier.carryCount;
        resHolder.Deduct(count);
        workerCtrl.resCarrier.AddResource(resHolder.Name(), count);
        this.DoneGetResNeedToMove(workerCtrl);

        //Find what building need these Resources
        workerTasks.taskBuildingCtrl = this.buildingCtrl;
        workerTasks.TaskAdd(TaskType.takingProductBack);
    }

    protected virtual void DoneGetResNeedToMove(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.taskBuildingCtrl = null;
    }

    protected virtual BuildingCtrl FindBuildingHasProductOld(WorkerCtrl workerCtrl)
    {
        int tryCount = 999;
        do
        {
            tryCount--;

            this.lastBuildingWorked++;
            if (lastBuildingWorked >= this.nearBuildings.Count)
            {
                this.lastBuildingWorked = 0;
                break;
            }

            BuildingCtrl nextBuilding = this.nearBuildings[this.lastBuildingWorked];
            if (nextBuilding.buildingType != BuildingType.workStation) continue;

            ResHolder resHolder = nextBuilding.warehouse.ResNeedToMove();
            if (resHolder == null) continue;

            workerCtrl.workerTasks.taskBuildingCtrl = nextBuilding;
            return nextBuilding;

        } while (tryCount > 0);

        return null;
    }

    protected virtual void BringResourceBack(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;
        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        workerTasks.taskBuildingCtrl = null;
        workerTasks.TaskCurrentDone();

        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.warehouse.AddResource(res.name, res.number);

        workerTasks.TaskAdd(TaskType.goToWorkStation);
    }

    protected virtual void BringMaterialToBuilding(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inHouse) workerTasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskBuildingCtrl = workerTasks.taskBuildingCtrl;

        if (workerCtrl.workerMovement.GetTarget() == null) workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);
        if (!workerCtrl.workerMovement.IsCloseToTarget()) return;

        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.warehouse.AddResource(res.name, res.number);

        workerTasks.taskBuildingCtrl = null;
        workerTasks.TaskCurrentDone();

        workerTasks.TaskAdd(TaskType.goToWorkStation);
    }
}
