using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private String audioClipFolder;
    protected AudioSource audioSource;
    public List<AudioClip> AudioClips { get; } = new List<AudioClip>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Resources.LoadAll<AudioClip>(audioClipFolder).ToList().ForEach(
                audioclip => AudioClips.Add(audioclip));
    }

    public void PlayAudio()
	{
        StartCoroutine(PlayDelayedAudio(AudioClips[Random.Range(0, AudioClips.Count)]));
    }

    public IEnumerator PlayDelayedAudio(AudioClip audioClip)
	{
        float delay = (Camera.main.transform.position - transform.position).magnitude * 0.005f;
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(audioClip);
    }
}
