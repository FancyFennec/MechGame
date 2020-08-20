using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioController : MonoBehaviour
{
    void Start()
    {
		float delay = (Camera.main.transform.position - transform.position).magnitude * 0.005f;
		GetComponent<AudioSource>().PlayDelayed(delay);
    }

    void Update()
    {
        
    }
}
