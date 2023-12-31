using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : BinBeha
{
	public static TreeManager instance;
	[SerializeField] protected List<GameObject> trees;

	protected override void Awake()
	{
		base.Awake();
		if (TreeManager.instance != null) Debug.LogError("Only 1 TreeManager allow to exist");
		TreeManager.instance = this;
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadTrees();
	}

	protected virtual void LoadTrees()
	{
		if (this.trees.Count > 0) return;
		foreach (Transform tree in transform)
		{
			this.TreeAdd(tree.gameObject);
		}
		Debug.Log(transform.name + ": LoadTrees", gameObject);
	}

	public virtual void TreeAdd(GameObject tree)
	{ 
		if (this.trees.Contains(tree)) return;	 
		this.trees.Add(tree);
		tree.transform.parent = transform;

		//to do, need check with current forest hut
	}

	public virtual bool TreeRemove(GameObject tree)
	{
		return this.trees.Remove(tree);
	}

	public virtual List<GameObject> Trees()
	{
		return this.trees;
	}
}
