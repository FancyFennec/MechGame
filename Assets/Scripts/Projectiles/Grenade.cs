using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioController))]
public class Grenade : ExplosiveProjectile
{
	AudioController audioController;
	public override void Start()
	{
		base.Start();
		audioController = GetComponent<AudioController>();
	}

	public void OnCollisionEnter(Collision collision)
	{
		audioController.PlayAudio();
		if (collision.collider.gameObject.GetComponent<Enemy>() != null)
		{
			ExplodeAt(collision.contacts[0].point);
		}
	}

	private void OnDestroy()
	{
		ExplodeAt(transform.position);
	}
}
