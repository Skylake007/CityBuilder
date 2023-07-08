using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoldCtrl : BinBeha
{
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadBuildType();
	}

	protected virtual void LoadBuildType()
	{
		PressableAbility pressable = GetComponentInParent<PressableAbility>();
		string buildType = pressable.GetBuildType();
		if (buildType == null) return;
		Debug.Log("Build type name: " + buildType);
		gameObject.name = buildType;
	}
}
