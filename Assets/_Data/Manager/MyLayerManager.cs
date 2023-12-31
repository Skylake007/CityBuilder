using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLayerManager : BinBeha
{
    public static MyLayerManager instance;

    [Header("Layers")]
    public int layerUI;
    public int layerWorker;
    public int layerGround;
    public int layerBuilding;
    public int layerTree;

    protected override void Awake()
    {
        if (MyLayerManager.instance != null) Debug.LogError("Only 1 MyLayerManager allow");
        MyLayerManager.instance = this;

        this.LoadComponents();

        //Physics.IgnoreLayerCollision(this.layerBullet, this.layerGround, true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.GetPlayers();
    }

    protected virtual void GetPlayers()
    {
        this.layerUI = LayerMask.NameToLayer("UI");
        this.layerWorker = LayerMask.NameToLayer("Worker");
        this.layerGround = LayerMask.NameToLayer("Ground");
        this.layerBuilding = LayerMask.NameToLayer("Building");
        this.layerTree = LayerMask.NameToLayer("Tree");

        if (this.layerUI < 0) Debug.LogError("Layer UI is mising");
        if (this.layerWorker < 0) Debug.LogError("Layer Worker is mising");
        if (this.layerGround < 0) Debug.LogError("Layer Ground is mising");
        if (this.layerBuilding < 0) Debug.LogError("Layer Building is mising");
        if (this.layerTree < 0) Debug.LogError("Layer Tree is mising");

        //Debug.Log(transform.name + ": GetPlayers", gameObject);
    }
}
