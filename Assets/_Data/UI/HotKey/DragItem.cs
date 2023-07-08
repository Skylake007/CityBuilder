﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : BinBeha, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	[SerializeField] protected Image image;
	[SerializeField] protected Transform realParent;
	public virtual void SetRealParent(Transform realParent)
	{
		this.realParent = realParent;
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadImage();
	}

	protected virtual void LoadImage()
	{
		if (this.image != null) return;
		this.image = GetComponent<Image>();
		Debug.Log(transform.name + ": LoadImage", gameObject);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		GodInput.Instance.DisableMouseRotation(); 

		Debug.Log("On begin drag");
		this.realParent = transform.parent;
		transform.parent = UIHotKeyCtrl.Instance.transform;
		//transform.parent = GodModeCtrl.Instance.MouseWorldPosition;
		this.image.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 mousePos = GodModeCtrl.Instance.MouseWorldPosition;// TODO: Will add instance before.
		Debug.Log("On drag mouse poi: " + mousePos);

		//mousePos.z = 0;
		transform.position = mousePos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GodInput.Instance.EnableMouseRotation();

		Debug.Log("On end drag");
		transform.parent = this.realParent;
		this.image.raycastTarget = true;
	}
}
