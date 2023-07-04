using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestroyable : BinBeha
{
	public virtual void Destroy()
	{
		PrefabManager.instance.Destroy(transform);
	}
}
