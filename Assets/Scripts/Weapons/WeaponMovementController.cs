﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementController : MonoBehaviour
{
    Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraTransform.forward), 10f * Time.deltaTime);
    }
}
