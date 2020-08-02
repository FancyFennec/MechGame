using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class HomingRocket: Projectile
{

	private Transform target;

	private GameObject explosion;
	private float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;
	private float CorrectionForce = 2.0f;

	public float Cooldown = 1.5f;
	public float CooldownTimer = 0f;
	protected bool isOnCooldown = true;

	public override void Start()
	{
		base.Start();
		explosion = Resources.Load<GameObject>("Explosion");
		target = GameObject.FindObjectsOfType<CharacterController>()[0].transform;
	}

	private void LateUpdate()
	{
		UpdateCooldownTimer();
		RotateTowardsTarget();
	}

	public override void FixedUpdate()
	{
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

	private void ExplodeAt(Vector3 pos)
	{
		Destroy(Instantiate(explosion, pos, Quaternion.identity), 2f);

		foreach (Collider collider in Physics.OverlapSphere(pos, ExplosionRadius))
		{
			float damagefactor = 1f - (collider.transform.position - pos).magnitude / ExplosionRadius;
			DamageHitCollider(collider, damagefactor);
			try
			{
				collider.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, pos, 6);
			}
			catch (Exception) { }
		}
		Destroy(this.gameObject);
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
