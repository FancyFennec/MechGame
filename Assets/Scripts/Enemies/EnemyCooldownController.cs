using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCooldownController : MonoBehaviour
{
    [Header("Cooldowns")]
    public float Cooldown = 1f;
    [System.NonSerialized]
    public float CooldownTimer = 0f;
    public bool IsOnCooldown { get; set; } = false;
    void Start(){}
    void LateUpdate()
    {
        UpdateCooldownTimer();
    }

    protected void UpdateCooldownTimer()
    {
        if (IsOnCooldown)
        {
            CooldownTimer = Mathf.Clamp(CooldownTimer + Time.deltaTime, 0f, Cooldown);
            if (CooldownTimer >= Cooldown)
            {
                CooldownTimer = 0f;
                IsOnCooldown = false;
            }
        }
    }
}
