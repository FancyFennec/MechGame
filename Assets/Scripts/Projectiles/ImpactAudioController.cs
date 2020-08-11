using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioController : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().PlayDelayed((Camera.main.transform.position - transform.position).magnitude * 0.005f);
    }

    void Update()
    {
        
    }
}
