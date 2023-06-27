using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawmillWH : Warehouse
{
	
	public override ResHolder ResNeedToMove()
	{
		ResHolder resHolder = this.GetResource(ResourceName.blank);
		if(resHolder.Current() > 0) return resHolder;
		return null;
	}
	public override ResHolder IsNeedRes(Resource res)
	{
		if (res.name != ResourceName.logwood) return null;

		ResHolder resHolder = this.GetResource(res.name);
		if (resHolder.IsMax()) return null;
		return resHolder;
	}
}
