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

	public enum Association
	{
		HOSTILE,
		ALLIED
	}

	public ProjectileType type;
	public Association friendlyness;
	private const float projectileForce = 100f;
	public int Damage = 200;

	private GameObject explosion;
	private float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;
	
	private Vector3 lastPosition;

	private GameObject blood;

	void Start()
	{
		blood = Resources.Load<GameObject>("Blood");
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
		if (Physics.Raycast(lastPosition, direction, out RaycastHit hit, direction.magnitude, GetLayerMask()))
		{
			if (ProjectileType.EXPLOSIVE.Equals(type))
			{
				ExplodeAt(hit.point);
			}
			else
			{
				ImpactAt(hit);
			}
		}

		lastPosition = transform.position;
	}

	private int GetLayerMask()
	{
		if (Association.HOSTILE.Equals(friendlyness))
		{
			return ~LayerMask.GetMask("Enemy");
		} else
		{
			return ~LayerMask.GetMask("Player");
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (ProjectileType.EXPLOSIVE.Equals(type))
		{
			ExplodeAt(collision.contacts[0].point);
		} else
		{
			ImpactAt(collision);
		}
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

	private bool DamageHitCollider(Collider collider)
	{
		return DamageHitCollider(collider, 1f);
	}

	private bool DamageHitCollider(Collider collider, float damagefactor)
	{
		if (Association.HOSTILE.Equals(friendlyness))
		{
			return DamagePlayer(collider, damagefactor);
		} else
		{
			return DamageEnemy(collider, damagefactor);
		}
	}

	private bool DamageEnemy(Collider collider, float damagefactor)
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
				catch (Exception)
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool DamagePlayer(Collider collider, float damagefactor)
	{
		try
		{
			collider.GetComponent<Health>().TakeDamage(damagefactor * Damage);
		}
		catch (Exception)
		{
			try
			{
				collider.GetComponentInChildren<Health>().TakeDamage(damagefactor * Damage);
			}
			catch (Exception)
			{
				try
				{
					collider.GetComponentInParent<Health>().TakeDamage(damagefactor * Damage);
				}
				catch (Exception)
				{
					return false;
				}
			}
		}
		return true;
	}

}
