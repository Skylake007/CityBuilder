using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BinBeha
{
    [SerializeField] protected int currentCameraIndex = 0;
    [SerializeField] protected List<Transform> cameraPrefabs;
  
    private static CameraManager instance;
    public static CameraManager Instance => instance;

	protected override void Awake()
	{
		base.Awake();
        if (CameraManager.instance != null) Debug.LogError("Only 1 CameraManaager allow to exist.");
        CameraManager.instance = this;
	}
	protected override void Start()
    {
        base.Start();
        this.HideAllPrefabs();
        this.SwitchCamera(); //Load first camera
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPrefabs();
    }

    protected virtual void LoadPrefabs()
    {
        if (this.cameraPrefabs.Count > 0) return;
        foreach (Transform child in transform)
        {
            this.cameraPrefabs.Add(child);
            //child.gameObject.SetActive(false);
        }
        Debug.Log(transform.name + ": LoadPrefabs", gameObject);
    }

    protected virtual void HideAllPrefabs()
    {
        foreach (Transform camera in this.cameraPrefabs)
        {
            camera.gameObject.SetActive(false);
        }
    }
    public virtual void SwitchCamera()
    {
        cameraPrefabs[currentCameraIndex].gameObject.SetActive(false);

        this.currentCameraIndex++;

        if (currentCameraIndex >= this.cameraPrefabs.Count)
        {
            currentCameraIndex = 0;
        }
        cameraPrefabs[currentCameraIndex].gameObject.SetActive(true);
    }

    public virtual int CameraIndex()
    {
        return this.currentCameraIndex;
    }
}
