using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

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
		Vector3 randomDirection = Vector3.up + new Vector3(
						Random.Range(-1f, 1f),
						0,
						Random.Range(-1f, 1f));

		GameObject spawnedEnemy = Instantiate<GameObject>(enemies[UnityEngine.Random.Range(0, enemies.Count)], 
			collision.contacts[0].point, 
			Quaternion.LookRotation(randomDirection, Vector3.up));
		spawnedEnemy.transform.parent = GameObject.Find("Enemies").transform;
	}
}
