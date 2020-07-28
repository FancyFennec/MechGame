using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Rocket: Projectile
{

	private GameObject explosion;
	private float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;

	public override void Start()
	{
		base.Start();
		explosion = Resources.Load<GameObject>("Explosion");
	}

	private void LateUpdate()
	{
		if (hasHitSomething)
		{
			ExplodeAt(hit.point);
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
}
