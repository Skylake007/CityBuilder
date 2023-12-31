using UnityEngine;

public class WorkerMovement : BinBeha
{
	public WorkerCtrl workerCtrl;
	[SerializeField] protected Transform target;
	public bool isWalking = false;
	public bool isWorking = false;
	public WorkingType workingType = WorkingType.chopTree;
	[SerializeField] protected float walkLimit = 0.7f;
	[SerializeField] protected float targetDistance = 0f;
	protected override void LoadComponents()
	{
		base.LoadComponents();
		this.LoadWorkerCtrl();
	}

	protected override void FixedUpdate()
	{
		this.Moving();
		this.Animating();
	}

	protected virtual void LoadWorkerCtrl()
	{
		if (this.workerCtrl != null) return;
		this.workerCtrl = GetComponent<WorkerCtrl>();
		Debug.LogWarning(transform.name + ": LoadWorkerCtrl", gameObject);
	}

	public virtual Transform GetTarget()
	{
		return this.target;
	}

	public virtual void SetTarget(Transform trans)
	{
		this.target = trans;

		if (this.target == null)
		{
			this.workerCtrl.navMeshAgent.enabled = false;
		}
		else
		{
			this.workerCtrl.navMeshAgent.enabled = true;
			this.IsCloseToTarget();
		}
	}

	protected virtual void Moving()
	{
		if (this.target == null || this.IsCloseToTarget())
		{
			this.isWalking = false;
			return;
		}

		this.isWalking = true;
		this.workerCtrl.navMeshAgent.SetDestination(this.target.position);
	}

	public virtual bool IsCloseToTarget()
	{
		if (this.target == null) return false;

		Vector3 targetPos = this.target.position;
		targetPos.y = transform.position.y;

		this.targetDistance = Vector3.Distance(transform.position, targetPos);
		return this.targetDistance < this.walkLimit;
	}

	protected virtual void Animating()
	{
		this.workerCtrl.animator.SetBool("isWalking", this.isWalking);
		this.workerCtrl.animator.SetBool("isWorking", this.isWorking);
		this.workerCtrl.animator.SetFloat("workingType", (float)this.workingType);
	}

	public virtual float TargetDistance()
	{
		return this.targetDistance;
	}
}
