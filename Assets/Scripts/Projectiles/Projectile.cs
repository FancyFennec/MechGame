using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Projectile : MonoBehaviour
{
	public enum Association
	{
		HOSTILE,
		ALLIED
	}

	public Association association;
	private const float projectileForce = 100f;
	public int Damage = 200;

	private Vector3 lastPosition;

	protected bool hasHitSomething = false;
	protected RaycastHit hit;

	public virtual void Start()
	{
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
		if (Physics.Raycast(lastPosition, direction, out hit, direction.magnitude, GetLayerMask()))
		{
			hasHitSomething = true;
		}
		lastPosition = transform.position;
	}

	private int GetLayerMask()
	{
		if (Association.HOSTILE.Equals(association))
		{
			return ~LayerMask.GetMask("Enemy");
		} else
		{
			return ~LayerMask.GetMask("Player");
		}
	}

	protected bool DamageHitCollider(Collider collider)
	{
		return DamageHitCollider(collider, 1f);
	}

	protected bool DamageHitCollider(Collider collider, float damagefactor)
	{
		if (Association.HOSTILE.Equals(association))
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
			collider.GetComponentInChildren<Health>().TakeDamage(damagefactor * Damage);
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}
}
