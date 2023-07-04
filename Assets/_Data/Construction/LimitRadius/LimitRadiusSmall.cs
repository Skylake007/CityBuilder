using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRadiusSmall : LimitRadius
{
	protected override void ResetValues()
	{
		base.ResetValues();
		this.buildRadius = 1;
	}
}
