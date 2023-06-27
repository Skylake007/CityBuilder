using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ResGenerator
{
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadResourceCreate();
		this.SetLimit();
	}

	protected virtual void LoadResourceCreate()
	{
		Resource resource = new Resource
		{
			name = ResourceName.logwood,
			number = 1
		};

		this.resCreate.Clear();
		this.resCreate.Add(resource);
	}

	protected virtual void SetLimit()
	{
		ResHolder resourceHolder = this.GetResource(ResourceName.logwood);
		resourceHolder.SetLimit(1);
	}
}
