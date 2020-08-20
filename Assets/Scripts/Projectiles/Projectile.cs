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
	public float projectileForce = 100f;
	public int Damage = 100;
	public bool useGravity = false;
	public float lifetime = 15f;

	private Vector3 lastPosition;
	protected Vector3 direction;

	protected RaycastHit hit;

	public virtual void Start()
	{
		lastPosition = transform.position;

		Rigidbody rigidbody = GetComponent<Rigidbody>();
		if(rigidbody != null)
		{
			rigidbody.AddForce(transform.forward * projectileForce);
			GetComponent<Rigidbody>().useGravity = useGravity;
		} else
		{
			Debug.LogError("No Rigidbody attached to Rocket script!");
		}

		Destroy(this.gameObject, lifetime);
	}

	public virtual void FixedUpdate()
	{
		direction = transform.position - lastPosition;
		lastPosition = transform.position;
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
		EnemyHealth health = collider.GetComponent<EnemyHealth>();
		if(health == null)
		{
			return false;
		}
		health.TakeDamage(damagefactor * Damage);
		return true;
	}

	private bool DamagePlayer(Collider collider, float damagefactor)
	{
		PlayerHealth health = collider.GetComponent<PlayerHealth>();
		if(health == null)
		{
			return false;
		}
		health.TakeDamage(damagefactor * Damage);
		return true;
	}
}
