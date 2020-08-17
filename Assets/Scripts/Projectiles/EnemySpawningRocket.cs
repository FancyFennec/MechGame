using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawningRocket : ExplosiveProjectile
{

	List<GameObject> enemies;

	public override void Start()
	{
		base.Start();
		enemies = Resources.LoadAll<GameObject>("Enemies").ToList();
	}

	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
		GameObject spawnedEnemy = Instantiate<GameObject>(enemies[UnityEngine.Random.Range(0, enemies.Count)], 
			collision.contacts[0].point, 
			Quaternion.identity);
		spawnedEnemy.transform.parent = GameObject.Find("Enemies").transform;
	}
}
