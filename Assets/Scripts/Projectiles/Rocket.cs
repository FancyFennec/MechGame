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
	}

	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
	}

	private void OnDestroy()
	{
		ExplodeAt(transform.position);
	}

	private void ExplodeAt(Vector3 pos)
	{
		Destroy(Instantiate(explosion, pos, Quaternion.identity), 2f);
		foreach (Collider collider in Physics.OverlapSphere(pos, ExplosionRadius))
		{
			Vector3 vector3 = collider.transform.position - pos;
			vector3.Scale(new Vector3(1, 0, 1));

			float damagefactor = Mathf.Clamp(1f - vector3.magnitude / ExplosionRadius, 0, 1);
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
