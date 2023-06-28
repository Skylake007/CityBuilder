using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeCtrl : BinBeha
{
    [Header("God Mode")]
    public Camera _camera;
    public GodMovement godMovement;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGodMovement();
        this.LoadCamera();
    }

    protected virtual void LoadGodMovement()
    {
        if (this.godMovement != null) return;
        this.godMovement = GetComponent<GodMovement>();
        Debug.Log(transform.name + ": LoadGodMovement", gameObject);
    }

    protected virtual void LoadCamera()
    {
        if (this._camera != null) return;
        this._camera = transform.Find("Camera").GetComponent<Camera>();
        this._camera.transform.rotation = Quaternion.Euler(this.godMovement.camView.x, this.godMovement.camView.y, this.godMovement.camView.z);
        Debug.Log(transform.name + ": LoadCamera", gameObject);
    }
}
