using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Bullet : Projectile
{
	private GameObject blood;

	public override void Start()
	{
		base.Start();
		blood = Resources.Load<GameObject>("Blood");
	}

	private void LateUpdate()
	{
		if (hasHitSomething)
		{
			ImpactAt(hit);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		ImpactAt(collision);
	}

	private void ImpactAt(Collision collision)
	{
		if (DamageHitCollider(collision.collider))
		{
			ContactPoint contacPoint = collision.contacts[0];
			Vector3 incomingVec = (contacPoint.point - transform.position).normalized;
			Vector3 reflectVec = Vector3.Reflect(incomingVec, contacPoint.normal);
			Destroy(Instantiate(blood, contacPoint.point, Quaternion.LookRotation(reflectVec)), 1f);
		}

		Destroy(this.gameObject);
	}

	private void ImpactAt(RaycastHit hit)
	{
		if (DamageHitCollider(hit.collider))
		{
			Vector3 incomingVec = (hit.point - transform.position).normalized;
			Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
			Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
		}

		Destroy(this.gameObject);
	}
}
