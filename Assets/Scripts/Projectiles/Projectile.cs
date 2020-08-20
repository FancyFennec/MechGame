using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
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
		rigidbody.AddForce(transform.forward * projectileForce);
		rigidbody.useGravity = useGravity;

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
		IHealth health = collider.GetComponent<IHealth>();
		if(health == null)
		{
			return false;
		} else
		{
			health.TakeDamage(damagefactor * Damage);
			return true;
		}
	}
}
