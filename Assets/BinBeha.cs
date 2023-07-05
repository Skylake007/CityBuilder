using UnityEngine;


public class BinBeha : MonoBehaviour
{
    protected virtual void Reset()
    {
        this.ResetValues();
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

    protected virtual void Update()
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

    protected virtual void ResetValues()
    {
        //For Overide
    }
}