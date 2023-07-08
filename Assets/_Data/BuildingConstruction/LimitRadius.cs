using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class LimitRadius : BinBeha
{
    [Header("Limit Radius")]
    [SerializeField] protected float buildRadius = 5f;
    [SerializeField] protected SphereCollider _collider;
	[SerializeField] protected Rigidbody _rigidbody;
    public List<GameObject> colliderObjs;

	protected override void OnEnable()
	{
		base.OnEnable();
		this.ResetColliderObjs();
	}

	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadCollider();
		this.LoadRigibody();
	}

	protected virtual void LoadCollider()
	{
		if (this._collider != null) return;
		this._collider = GetComponent<SphereCollider>();
		this._collider.radius = this.buildRadius;
		this._collider.isTrigger = true;
		Debug.Log(transform.name + ": LoadCollider", gameObject);
	}

	protected virtual void LoadRigibody()
	{
		if (this._rigidbody != null) return;
		this._rigidbody = GetComponent<Rigidbody>();
		this._rigidbody.useGravity = false;
		Debug.Log(transform.name + ": LoadRigibody", gameObject);
	}

	public virtual bool IsCollided()
	{
		if (colliderObjs.Count < 1) return false;

		if (this.IsCollidedWithBuilding()) return true;

		List<int> layers = new List<int>
		{
			MyLayerManager.instance.layerTree
		};

		this.CleanByLayers(layers);

		return false;
	}

	public virtual bool IsCollidedWithBuilding()
	{
		foreach (GameObject colliderObj in this.colliderObjs)
		{
			if (colliderObj.layer == MyLayerManager.instance.layerBuilding) return true;
		}

		return false;
	}

	public virtual void CleanByLayers(List<int> layers)
	{
		GameObject colObj;
		int i = 0;
		do
		{ //follow cau truc nay de khong bi loi khi huy obj
			colObj = this.colliderObjs[i];
			if (layers.Contains(colObj.layer))
			{
				this.colliderObjs.RemoveAt(i);
				this.CleanObj(colObj);
				i = 0;
				continue;
			}
			i++;
		} while (i < this.colliderObjs.Count);
	}

	private void OnTriggerEnter(Collider other)
	{
		bool canCollide = false;
		int colliderLayer = other.gameObject.layer;

		if (colliderLayer == MyLayerManager.instance.layerBuilding) canCollide = true;
		if (colliderLayer == MyLayerManager.instance.layerTree) canCollide = true;

		if(!canCollide) return;

		this.colliderObjs.Add(other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		this.colliderObjs.Remove(other.gameObject);
	}

	protected virtual void CleanObj(GameObject obj)
	{
		BuildDestroyable buildDestroyable = obj.GetComponent<BuildDestroyable>();
		if (buildDestroyable == null) return;

		buildDestroyable.Destroy();
	}

	protected virtual void ResetColliderObjs()
	{
		this.colliderObjs.Clear();
	}

}
