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
	}

	void FixedUpdate()
	{
		Vector3 direction = transform.position - lastPosition;
		if (Physics.Raycast(lastPosition, direction, out RaycastHit hit, direction.magnitude, ~LayerMask.GetMask("Player")))
		{
			ExplodeAt(hit);
		}

		lastPosition = transform.position;
	}

	private void ExplodeAt(RaycastHit hit)
	{
		Destroy(Instantiate(explosion, hit.point, Quaternion.identity), 2f);

		foreach (Collider collider in Physics.OverlapSphere(hit.point, ExplosionRadius))
		{
			try
			{
				collider.GetComponent<Enemy>().TakeDamage(ExplosionDamage);
			}
			catch (Exception) { }
			try
			{
				collider.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, hit.point, 6);
			} 
			catch (Exception) {}
		}
		Destroy(this.gameObject);
	}
}
