using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTree : AbstractConstruction
{
	protected override void LoadBuildNames()
	{
		if (this.buildNames.Count > 0) return;
		this.buildNames.Add("Tree_1");
		this.buildNames.Add("Tree_2");
		this.buildNames.Add("Tree_3");
		this.buildNames.Add("Tree_4");
		this.buildNames.Add("Tree_5");
		Debug.Log(transform.name + " LoadBuildNames", gameObject);
	}

	protected override Transform FinishBuild()
	{
		Transform newBuild = base.FinishBuild();
		TreeManager.instance.TreeAdd(newBuild.gameObject);
		return newBuild;
	}
}
