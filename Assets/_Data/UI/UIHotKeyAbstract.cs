using UnityEngine;
public class UIHotKeyAbstract : BinBeha
{
	[SerializeField] protected UIHotKeyCtrl hotKeyCtrl;

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadUIKetCtrl();
	}
	protected virtual void LoadUIKetCtrl()
	{
		if (this.hotKeyCtrl != null) return;
		this.hotKeyCtrl = transform.parent.GetComponent<UIHotKeyCtrl>();
		Debug.LogWarning(transform.name + ": LoadUIKetCtrl" + gameObject);
	}
}
