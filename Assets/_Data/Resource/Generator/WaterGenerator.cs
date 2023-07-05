using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : ResGenerator
{
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadResourceCreate();
		this.SetLimit();
	}

	protected virtual void LoadResourceCreate()
	{
		Resource res = new Resource
		{
			name = ResourceName.logwood,
			number = 1
		};

		this.resCreate.Clear();
		this.resCreate.Add(res);
	}

	protected virtual void SetLimit()
	{
		ResHolder resourceHolder = this.GetResource(ResourceName.water);
		resourceHolder.SetLimit(7);
	}
}
