using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BinBeha : MonoBehaviour
{
    protected virtual void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Start()
    {
        //For Overide
    }

    protected virtual void FixedUpdate()
    {
        //For Overide
    }

    protected virtual void OnDisable()
    {
        //For Overide
    }

    protected virtual void OnEnable()
    {
        //For Overide
    }

    protected virtual void LoadComponents()
    {
        //For Overide
    }
}