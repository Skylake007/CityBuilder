using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionCtrl : BinBeha
{
    public Image image;
    public LimitRadius limitRadius;
    public AbstractConstruction abstractConstruction;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLimitRadius();
        this.LoadAbstractConstruction();
        this.LoadImgIns();
    }

    protected virtual void LoadLimitRadius()
    {
        if (this.limitRadius != null) return;
        this.limitRadius = GetComponent<LimitRadius>();
        Debug.Log(transform.name + ": LoadLimitRadius", gameObject);
    }

    protected virtual void LoadAbstractConstruction()
    {
        if (this.abstractConstruction != null) return;
        this.abstractConstruction = GetComponent<AbstractConstruction>();
        Debug.Log(transform.name + ": LoadAbstractConstruction", gameObject);
    }

    protected virtual void LoadImgIns()
    {
        if (this.image != null) return;
        this.image = GetComponent<Image>();
        Debug.Log(transform.name + ": LoadImage", gameObject);
    }
}
