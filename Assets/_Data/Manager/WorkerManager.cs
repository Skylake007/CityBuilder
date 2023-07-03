using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : BinBeha
{
    public static WorkerManager instance;

	protected override void Awake()
	{
		base.Awake();
        if (WorkerManager.instance != null) Debug.LogError("Only 1 WorkerManager allow to exist");
        WorkerManager.instance = this;
	}
}
