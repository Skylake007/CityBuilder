using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusLimitSmall : LimitRadius
{
	protected override void ResetValues()
	{
		base.ResetValues();
		this.buildRadius = 1;
	}
}
