using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildForestHut : BuildBuilding
{
	protected override void LoadResRequires()
	{
		if (this.resRequires.Count > 0) return;
		this.resRequires.Add(new Resource { name = ResourceName.blank, number = 5 });
		Debug.Log(transform.name + ": LoadResRequires", gameObject);
	}
}
