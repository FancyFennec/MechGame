﻿using UnityEngine;
using System.Collections;
using System;

public class Bullet : Projectile
{
	private GameObject blood;

	public override void Start()
	{
		base.Start();
		blood = Resources.Load<GameObject>("Blood");
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

		Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			rigidbody.AddForce((collision.transform.position - transform.position).normalized * 100f);
		}

		RemoveTrail();
		Destroy(this.gameObject);
	}

	private void RemoveTrail()
	{
		Transform trail = transform.Find("Trail");
		if(trail != null)
		{
			trail.parent = null;
			Destroy(trail.gameObject, 2f);
		}
	}
}
