using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class ExplosiveProjectile : Projectile, IHealth
{
	protected GameObject explosion;
	private readonly float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;
	private Boolean hasExploded = false;

	public override void Start()
	{
		base.Start();
		explosion = Resources.Load<GameObject>("BigExplosionEffect");
	}

	public void ExplodeAt(Vector3 pos)
	{
		Destroy(Instantiate(this.explosion, pos, Quaternion.identity), 3f);
		foreach (Collider collider in Physics.OverlapSphere(pos, ExplosionRadius))
		{
			Vector3 vector3 = collider.transform.position - pos;
			vector3.Scale(new Vector3(1, 0, 1));

			float damagefactor = Mathf.Clamp(1f - vector3.magnitude / ExplosionRadius, 0, 1);
			DamageHitCollider(collider, damagefactor);

			Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
			if(rigidbody != null)
			{
				rigidbody.AddExplosionForce(ExplosionForce, pos, 6);
			}
		}
		Destroy(this.gameObject);
	}

	public void TakeDamage(float damage)
	{
		if (!hasExploded)
		{
			hasExploded = true;
			ExplodeAt(transform.position);
		}
		
	}
}
