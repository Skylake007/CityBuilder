using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : BinBeha, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] protected PressableAbility pressable;
	[SerializeField] protected bool isChoosePlaceFirstPersion = false;
	[SerializeField] protected Image image;
	[SerializeField] protected Transform realParent;
	[SerializeField] protected bool isTouchingUI = false;

	public virtual void SetRealParent(Transform realParent)
	{
		this.realParent = realParent;
	}

	protected override void Update()
	{
		base.Update();
		this.CheckTouchUI();
		this.HidenMousePointer();
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadImage();
		this.LoadBuildTypeName();
	}

	protected virtual void LoadImage()
	{
		if (this.image != null) return;
		this.image = GetComponent<Image>();
		Debug.Log(transform.name + ": LoadImage", gameObject);
	}

	protected virtual void HidenMousePointer()
	{ 
		
	}

	protected virtual void LoadBuildTypeName()
	{
		if (this.pressable != null) return;
		this.pressable = GetComponentInChildren<PressableAbility>();
		string buildName = pressable.GetBuildType();
		Debug.Log(transform.name + ": LoadBuildName " + buildName, gameObject);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		GodInput.Instance.DisableMouseRotation();
		Debug.Log("On begin drag");
		this.realParent = transform.parent;
		transform.parent = UIHotKeyCtrl.Instance.transform;
		//transform.parent = GodModeCtrl.Instance.MouseWorldPosition;
		this.image.raycastTarget = false;

		BuildManager.instance.CurrentBuildSet(this.pressable.GetBuildType());

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
		//BuildManager.instance.CurrentBuildClear();
	}

	protected void CheckTouchUI()
	{
		Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
		//Diem cham giua con chuot va mat phang

		int mask = (1 << MyLayerManager.instance.layerUI);
		if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
		{
			this.isTouchingUI = true;
			Debug.Log("Ray hit UI");
		}
		else { this.isTouchingUI = false; }
	}
}