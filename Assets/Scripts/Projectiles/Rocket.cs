using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;

public class Rocket: ExplosiveProjectile
{
	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
	}
}
