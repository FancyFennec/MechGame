using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Rocket : MonoBehaviour
{
	private const float rocketForce = 100f;

	private GameObject explosion;
	private const float ExplosionRadius = 2f;
	private const int ExplosionForce = 500;
	private const int ExplosionDamage = 200;
	
	private Vector3 lastPosition;

	void Start()
	{
		explosion = Resources.Load<GameObject>("Explosion");
		lastPosition = transform.position;
		try
		{
			GetComponent<Rigidbody>().AddForce(transform.forward * rocketForce);
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
			Debug.Log("Hit something");
			ExplodeAt(hit.point);
		}

		lastPosition = transform.position;
	}

	private void ExplodeAt(Vector3 pos)
	{
		Destroy(Instantiate(explosion, pos, Quaternion.identity), 2f);

		foreach (Collider collider in Physics.OverlapSphere(pos, ExplosionRadius))
		{
			float damagefactor = 1f - (collider.transform.position - pos).magnitude / ExplosionRadius;
			try
			{
				collider.GetComponent<Enemy>().TakeDamage(damagefactor * ExplosionDamage);
			}
			catch (Exception) {
				try
				{
					collider.GetComponentInChildren<Enemy>().TakeDamage(damagefactor * ExplosionDamage);
				}
				catch (Exception) {
					try
					{
						collider.GetComponentInParent<Enemy>().TakeDamage(damagefactor * ExplosionDamage);
					}
					catch (Exception) { }
				}
			}
			try
			{
				collider.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, pos, 6);
			} 
			catch (Exception) {}
		}
		Destroy(this.gameObject);
	}

	public void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Colliding");
		ExplodeAt(collision.contacts[0].point);
	}
}
