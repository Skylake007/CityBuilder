using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResGenerator : Warehouse
{
	[Header("ResGenerator")]
	[SerializeField] protected List<Resource> resCreate;
	[SerializeField] protected List<Resource> resRequire;
	public bool canCreate = true;

	[SerializeField] public float percentNextLevel = 0f;
	[SerializeField] protected float createTimer = 0f;
	[SerializeField] protected float createDelay = 60f;

	protected override void FixedUpdate()
	{
		this.Creating();
	}

	protected virtual void Creating()
	{
		if (!this.canCreate) return;

		this.createTimer += Time.fixedDeltaTime;
		this.percentNextLevel = createTimer / createDelay * 100;
		if (this.createTimer < this.createDelay) return;
		this.percentNextLevel = 0;
		this.createTimer = 0;

		if (!this.IsRequireEnough()) return;

		foreach (Resource res in this.resCreate)
		{
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

	public virtual List<Resource> TakeAll()
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
