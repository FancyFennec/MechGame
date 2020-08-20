using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	protected Rigidbody rb;
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

		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * projectileForce);
		rb.useGravity = useGravity;

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
