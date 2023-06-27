using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : BinBeha
{
	[Header("Wearhouse")]
	public BuildingType buildingType = BuildingType.workStation;
	[SerializeField] protected bool isFull = false;
	[SerializeField] protected List<ResHolder> resHolders;

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.isFull = this.IsFull();
	}
	protected override void LoadComponents()
	{
		this.LoadHolders();
	}

	protected virtual void LoadHolders()
	{
		if (this.resHolders.Count > 0) return;

		Transform res = transform.Find("Resource");
		foreach (Transform resTran in res)
		{
			Debug.Log("Res tran: " + resTran);
			ResHolder resourceHolder = resTran.GetComponent<ResHolder>();

			if (resourceHolder == null) continue;
			this.resHolders.Add(resourceHolder);
		}

		Debug.Log(transform.name + ": LoadHolders");
	}

	public virtual ResHolder GetResource(ResourceName name)
	{
		return this.resHolders.Find((holder) => holder.Name() == name);
	}

	public virtual void AddByList(List<Resource> addResources)
	{
		foreach (Resource addResource in addResources)
		{
			this.AddResource(addResource.name, addResource.number);
		}
	}

	public virtual ResHolder AddResource(ResourceName resourceName, float number)
	{
		ResHolder res = this.GetResource(resourceName);
		res.Add(number);
		return res;
	}

	public virtual ResHolder RemoveResource(ResourceName resourceName, float number)
	{
		ResHolder res = this.GetResource(resourceName);
		res.Deduct(number);
		return res;
	}

	public virtual bool IsFull()
	{
		foreach (ResHolder resHolder in this.resHolders)
		{
			if (!resHolder.IsMax()) return false;
		}
		return true;
	}
}