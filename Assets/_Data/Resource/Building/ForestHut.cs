using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestHut : Warehouse
{
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.buildingType = BuildingType.workStation
		;
	}
}