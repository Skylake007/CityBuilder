using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPressAlpha : UIHotKeyAbstract
{

    protected override void Update()
	{
        this.CheckAlphaIsPress();
	}

    protected virtual void CheckAlphaIsPress()
    {
        if (InputHotKeyManager.Instance.isAlpha1) this.Press(0);
        if (InputHotKeyManager.Instance.isAlpha2) this.Press(1);
        if (InputHotKeyManager.Instance.isAlpha3) this.Press(2);
        if (InputHotKeyManager.Instance.isAlpha4) this.Press(3);
        if (InputHotKeyManager.Instance.isAlpha5) this.Press(4);
        if (InputHotKeyManager.Instance.isAlpha6) this.Press(5);
        if (InputHotKeyManager.Instance.isAlpha7) this.Press(6);


        if (InputHotKeyManager.Instance.isAlphaZ || InputHotKeyManager.Instance.isAlphaESC)
        {
            //Unchecked
            BuildManager.instance.CurrentBuildClear();
        }


        if (InputHotKeyManager.Instance.isAlphaX)
        {
            //Destroy building
            BuildManager.instance.CurrentBuildSet("BuildDestroy");
        }

        if (InputHotKeyManager.Instance.isAlphaC)
        { 
            //Switch camera
            Debug.Log("Switch camera");
            CameraManager.Instance.SwitchCamera();
        }

        if (InputHotKeyManager.Instance.isAlphaLeftAlt)
        {
            InputHotKeyManager.Instance.isHidenPointer = true;
        }   
        else {
            InputHotKeyManager.Instance.isHidenPointer = false;
        }
    }

    protected virtual void Press(int alpha)
    {
        //Debug.Log("Alpha: " + alpha);
        ItemSlot itemSlot = this.hotKeyCtrl.itemSlots[alpha];
        Pressable pressable = itemSlot.GetComponentInChildren<Pressable>();
        if (pressable == null) return;
        pressable.Pressed();
    }
}