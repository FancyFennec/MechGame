using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Projectile : MonoBehaviour
{
	public enum ProjectileType
	{
		EXPLOSIVE,
		EMPALING,
		PENETRATING
	}

	public ProjectileType type;
	private const float projectileForce = 100f;
	public int Damage = 200;

	private GameObject explosion;
	private float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;
	
	private Vector3 lastPosition;

	void Start()
	{
		explosion = Resources.Load<GameObject>("Explosion");
		lastPosition = transform.position;
		try
		{
			GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);
		} catch (Exception)
		{
			Debug.LogError("No Rigidbody attached to Rocket script!");
		}
		Destroy(this.gameObject, 5f);
	}

	void FixedUpdate()
	{
		Vector3 direction = transform.position - lastPosition;
		if (Physics.Raycast(lastPosition, direction, out RaycastHit hit, direction.magnitude, ~LayerMask.GetMask("Player")))
		{
			if (ProjectileType.EXPLOSIVE.Equals(type))
			{
				ExplodeAt(hit.point);
			} else
			{
				ImpactAt(hit.collider);
			}
		}

		lastPosition = transform.position;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (ProjectileType.EXPLOSIVE.Equals(type))
		{
			ExplodeAt(collision.contacts[0].point);
		} else
		{
			ImpactAt(collision.collider);
		}
	}

	private void ImpactAt(Collider collider)
	{
		DamageHitCollider(collider);
		Destroy(this.gameObject);
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

	private void DamageHitCollider(Collider collider)
	{
		DamageHitCollider(collider, 1f);
	}

	private void DamageHitCollider(Collider collider, float damagefactor)
	{
		try
		{
			collider.GetComponent<Enemy>().TakeDamage(damagefactor * Damage);
		}
		catch (Exception)
		{
			try
			{
				collider.GetComponentInChildren<Enemy>().TakeDamage(damagefactor * Damage);
			}
			catch (Exception)
			{
				try
				{
					collider.GetComponentInParent<Enemy>().TakeDamage(damagefactor * Damage);
				}
				catch (Exception) { }
			}
		}
	}
}
