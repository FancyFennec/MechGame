using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class Rocket: ExplosiveProjectile
{

	List<AudioClip> audioClips;
	AudioSource audioSource;

	public override void Start()
	{
		base.Start();
		audioSource = GetComponent<AudioSource>();
		audioClips = Resources.LoadAll<AudioClip>("Audio/Rocket").ToList();
		audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
	}

	public void OnCollisionEnter(Collision collision)
	{
		ExplodeAt(collision.contacts[0].point);
	}
}
