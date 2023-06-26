using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
	public static ResManager instance;

	[SerializeField] protected List<Resource> resources;

	protected void Awake()
	{
		if (ResManager.instance != null) Debug.LogError("Only 1 Resource allown");
		ResManager.instance = this;
	}

	public virtual Resource AddResource(ResourceName resName, int number)
	{
		Debug.Log("Add: " + resName + " " + number);
		Resource res = this.GetResourceByName(resName);
		res.number += number;

		return res;
	}

	public virtual Resource GetResourceByName(ResourceName resName)
	{
		Resource res = this.resources.Find((x) => x.name == resName);
		if (res == null)
		{
			res = new Resource();
			res.name = resName;
			res.number = 0;

			this.resources.Add(res);
		}
		return res;

	}
}
