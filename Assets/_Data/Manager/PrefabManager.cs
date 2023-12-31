using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : BinBeha
{
    public static PrefabManager instance;
    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected int currentCameraIndex = 0;


    protected override void Awake()
	{
		base.Awake();
        if (PrefabManager.instance != null) Debug.LogWarning("Only 1 PrefabManager allow to exist");
        PrefabManager.instance = this;
	}

	protected override void Start()
	{
		base.Start();
        this.HideAllPrefabs();
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
        this.LoadPrefabs();
	}

    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;
        foreach (Transform child in transform)
        {
            this.prefabs.Add(child);
            //child.gameObject.SetActive(false);
        }
        Debug.Log(transform.name + ": LoadPrefabs", gameObject);
    }

    protected virtual void HideAllPrefabs()
    {
        foreach (Transform prefab in this.prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }

    public virtual Transform Instantiate(string prefabName)
    {
        Debug.Log(transform.name + ": " + prefabName);
        Transform prefab = this.Get(prefabName);
        GameObject newObj = Instantiate(prefab.gameObject);
        newObj.name = prefab.name;

        return newObj.transform;
    }

    public virtual void Destroy(Transform transform)
    {
        Destroy(transform.gameObject);
    }

    public virtual Transform Get(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name != prefabName) continue;
            return prefab;
        }
        return null;
    }
}
