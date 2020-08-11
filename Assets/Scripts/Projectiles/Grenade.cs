using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Grenade : ExplosiveProjectile
{
	public void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.gameObject.GetComponent<Enemy>() != null)
		{
			ExplodeAt(collision.contacts[0].point);
		}
	}

	private void OnDestroy()
	{
		ExplodeAt(transform.position);
	}
}
