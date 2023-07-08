using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : BinBeha, IDropHandler
	{
	public void OnDrop(PointerEventData eventData)
		{
		if (transform.childCount > 0) return;// Check item slot have item yet?
		Debug.Log("OnDrop");
		GameObject dropObj = eventData.pointerDrag;
		DragItem dragItem = dropObj.GetComponent<DragItem>();
		dragItem.SetRealParent(transform);
	}
}
