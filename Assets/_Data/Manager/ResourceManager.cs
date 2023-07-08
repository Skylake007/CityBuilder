using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : BinBeha
{
	public static ResourceManager instance;
	[SerializeField] protected int Wood = 0;

	protected override void Awake()
	{
		base.Awake();
		if (ResourceManager.instance != null) Debug.LogError("Only 1 ResourceManager allow to exist");
		ResourceManager.instance = this;
	}

	public virtual int GetWood()
	{
		return this.Wood;
	}
}

