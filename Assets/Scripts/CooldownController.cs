using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Boo.Lang;
using System.Runtime.CompilerServices;

public class CooldownController : MonoBehaviour
{
    [Header("Cooldowns")]
    public float Cooldown = 1f;

    private bool isOnCooldown = false;
    public bool IsOnCooldown
    {
        get { return isOnCooldown; }
        set {
            isOnCooldown = value;
            if (value)
			{
                StartCooldown();
            }
        }
    }

    public void StartCooldown()
    {
        StartCoroutine(ResetCooldown());
    }
    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(Cooldown);
        IsOnCooldown = false;
    }
}
