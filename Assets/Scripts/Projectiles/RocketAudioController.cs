using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RocketAudioController : MonoBehaviour
{

	List<AudioClip> audioClips;
	AudioSource audioSource;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioClips = Resources.LoadAll<AudioClip>("Audio/Rocket").ToList();
		audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
		audioSource.Play();
	}
}
