using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : BinBeha
{
	public static ConstructionManager instance;
	[SerializeField] protected List<AbstractConstruction> constructions;

	protected override void Awake()
	{
		base.Awake();
		if (ConstructionManager.instance != null) Debug.LogError("Only 1 ConstructionManager allow to exist");
		ConstructionManager.instance = this;
	}

	public virtual void AddConstruction(AbstractConstruction abstractConstruction)
	{
		//Debug.LogError(abstractConstruction);
		this.constructions.Add(abstractConstruction);
		abstractConstruction.transform.parent = transform;
	}

	public virtual AbstractConstruction GetConstruction()
	{
		foreach (AbstractConstruction construction in this.constructions)
		{
			if (construction.builder != null) continue;
			if (!construction.HasEnoughtResource()) return construction;
		}
		return null;
	}
}
