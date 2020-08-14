using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Timeline;
using Boo.Lang;
using System.Linq;

public class EnemySpawningRocket : ExplosiveProjectile
{

	System.Collections.Generic.List<UnityEngine.Object> enemies;

	public override void Start()
	{
		base.Start();
		enemies = Resources.LoadAll("Enemies").ToList();
	}

	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
		Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Count)], 
			collision.contacts[0].point, 
			Quaternion.identity);
	}
}
