using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class HomingRocket: ExplosiveProjectile
{

	private Transform target;

	private readonly float CorrectionForce = 2.0f;

	public float Cooldown = 1.5f;
	public float CooldownTimer = 0f;
	protected bool isOnCooldown = true;

	public override void Start()
	{
		base.Start();
		target = Camera.main.transform;
	}

	private void LateUpdate()
	{
		UpdateCooldownTimer();
		RotateTowardsTarget();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!isOnCooldown)
		{
			Vector3 correctionDirection = ((target.position - transform.position).normalized - transform.forward).normalized;
			GetComponent<Rigidbody>().AddForce(correctionDirection * CorrectionForce);
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

	protected void UpdateCooldownTimer()
	{
		if (isOnCooldown)
		{
			if (CooldownTimer < Cooldown)
			{
				CooldownTimer = Mathf.Clamp(CooldownTimer + Time.deltaTime, 0f, Cooldown);
			}
			else
			{
				CooldownTimer = 0f;
				isOnCooldown = false;
			}
		}
	}
}
