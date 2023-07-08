using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestroyTree : BuildDestroyable
{
	public override void Destroy()
	{
		TreeManager.instance.TreeRemove(gameObject);
		base.Destroy();
	}
}
