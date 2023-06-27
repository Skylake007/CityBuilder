using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : ResGenerator
{
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadResCreate();
		this.SetLimit();

	}

	protected virtual void LoadResCreate()
	{
		Resource resource = new Resource
		{
			name = ResourceName.water,
			number = 1
		};

		this.resCreate.Clear();
		this.resCreate.Add(resource);
	}

	protected virtual void SetLimit()
	{
		ResHolder resourceHolder = this.GetResource(ResourceName.water);
		resourceHolder.SetLimit(7);
	}
}
