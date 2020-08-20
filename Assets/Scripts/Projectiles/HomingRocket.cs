using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CooldownController))]
public class HomingRocket: ExplosiveProjectile
{
	private CooldownController cooldownController;
	private Transform target;

	[SerializeField] private float CorrectionForce = 7.0f;

	public override void Start()
	{
		base.Start();
		target = Camera.main.transform;
		cooldownController = GetComponent<CooldownController>();
		cooldownController.IsOnCooldown = true;
	}

	private void LateUpdate()
	{
		RotateTowardsTarget();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!cooldownController.IsOnCooldown)
		{
			Vector3 targetDirection = (target.position - transform.position).normalized;
			Vector3 correctionDirection = (targetDirection - transform.forward).normalized;
			float correctionFactor = Vector3.Dot(targetDirection, correctionDirection);
			rb.AddForce(correctionDirection * correctionFactor * CorrectionForce);
			rb.AddForce(targetDirection);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
	}

	public void RotateTowardsTarget()
	{
		transform.rotation = Quaternion.Lerp(
			Quaternion.LookRotation(direction, Vector3.up),
			transform.rotation,
			0.95f);
	}
}
