using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResHolder : BinBeha
{
	[Header("Res Holder")]
	[SerializeField] protected ResourceName resName;
	[SerializeField] public float resCurrent = 0f;
	[SerializeField] public float resMax = Mathf.Infinity;

	protected override void LoadComponents()
	{
		this.LoadResName();
	}

	protected virtual void LoadResName()
	{
		if (this.resName != ResourceName.noResource) return;

		string name = transform.name;
		this.resName = ResNameParser.FromString(name);
		Debug.Log(transform.name + ": LoadResourceName");
	}

	public virtual ResourceName Name()
	{
		return this.resName;
	}

	public virtual float Add(float number)
	{
		this.resCurrent += number;

		//if (this.resCurrent > this.resMax) this.resCurrent = this.resMax;
		return this.resCurrent;
	}
	public virtual float Deduct(float number)
	{
		//TODO: fix issue less than 0 
		return this.Add(-number);
	}

	public virtual float Current()
	{
		return this.resCurrent;
	}

	public virtual float TakeAll()
	{
		float take = this.resCurrent;
		this.resCurrent = 0;
		return take;
	}

	public virtual bool IsMax()
	{
		return this.resCurrent >= this.resMax;
	}

	public virtual void SetLimit(int resLimit)
	{
		this.resMax = resLimit;
	}
}
