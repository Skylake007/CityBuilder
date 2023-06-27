using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResGenerator : Warehouse
{
	[SerializeField] protected List<Resource> resCreate;
	[SerializeField] protected List<Resource> resRequire;
	public bool canCreate = true;
	[SerializeField] protected float createTimer = 0f;
	[SerializeField] protected float createDelay = 2f;

	protected override void FixedUpdate()
	{
		this.Creating();
	}

	protected virtual void Creating()
	{
		this.createTimer += Time.fixedDeltaTime;
		if (this.createTimer < this.createDelay) return;
		this.createTimer = 0;

		if (!this.IsRequireEnough()) return;

		foreach (Resource res in this.resCreate)
		{
			//ResourceHolder resourceHolder = this.resourceHolders.Find((holder) => holder.Name() == res.name);
			ResHolder resourceHolder = this.GetResource(res.name);
			resourceHolder.Add(res.number);
		}
	}
	public virtual bool IsAllResMax()
	{
		foreach (ResHolder resHolder in this.resHolders)
		{
			if (resHolder.IsMax() == false) return false;
		}

		return true;
	}

	public virtual float GetCreateDelay()
	{
		return this.createDelay;
	}

	protected virtual bool IsRequireEnough()
	{
		if (this.resRequire.Count < 1) return true;
		//to do this is not done yet
		return false;
	}

	public virtual List<Resource> TakeAll(ResourceName resourceName)
	{
		List<Resource> resources = new List<Resource>();
		foreach (ResHolder resHolder in this.resHolders)
		{
			Resource newResource = new Resource
			{
				name = resHolder.Name(),
				number = resHolder.TakeAll()
			};

			resources.Add(newResource);
		}

		return resources;
	}
}
