using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHotKeyManager : MonoBehaviour
{
    private static InputHotKeyManager instance;
    public static InputHotKeyManager Instance => instance;

    public bool isAlpha1 = false;
    public bool isAlpha2 = false;
    public bool isAlpha3 = false;
    public bool isAlpha4 = false;
    public bool isAlpha5 = false;
    public bool isAlpha6 = false;
    public bool isAlpha7 = false;

    public bool isAlphaX = false; //Destroy
    public bool isAlphaZ = false; //Unchecked 1
    public bool isAlphaC = false; //SwtichCamera
    public bool isAlphaLeftAlt = false; //ShowMousePoiter
    public bool isAlphaESC = false; //Unchecked 2

    public bool isHidenPointer = true;

    void Awake()
    {
        if (InputHotKeyManager.instance != null) Debug.LogError("Only 1 InputHotKeyManager allow to exist");
        InputHotKeyManager.instance = this;
    }

    private void Update()
    {
        this.GetHotKeyPress();
        this.HidenPointer();
    }

    protected virtual void GetHotKeyPress()
    {
        this.isAlpha1 = Input.GetKeyDown(KeyCode.Alpha1);
        this.isAlpha2 = Input.GetKeyDown(KeyCode.Alpha2);
        this.isAlpha3 = Input.GetKeyDown(KeyCode.Alpha3);
        this.isAlpha4 = Input.GetKeyDown(KeyCode.Alpha4);
        this.isAlpha5 = Input.GetKeyDown(KeyCode.Alpha5);
        this.isAlpha6 = Input.GetKeyDown(KeyCode.Alpha6);
        this.isAlpha7 = Input.GetKeyDown(KeyCode.Alpha7);
        this.isAlphaC = Input.GetKeyDown(KeyCode.C);
        this.isAlphaZ = Input.GetKeyDown(KeyCode.Z);
        this.isAlphaX = Input.GetKeyDown(KeyCode.X);
        this.isAlphaESC = Input.GetKeyDown(KeyCode.Escape);
        this.isAlphaLeftAlt = Input.GetKey(KeyCode.LeftAlt);
    }

    protected virtual void HidenPointer()
    {
        if (isHidenPointer)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}