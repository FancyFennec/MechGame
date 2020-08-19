using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public static Health instance;
    public int MaxHealth { get; } = 200;
    public int CurrentHealth { get; private set; } = 100;

    public event Action PlayerDiedEvent;
    public event Action PlayerDamageTakenEvent;

	private void Awake()
	{
        instance = this;
	}
	void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        PlayerDamageTakenEvent.Invoke();
        CurrentHealth = Mathf.FloorToInt(Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth));
        if (CurrentHealth == 0f)
        {
            PlayerDiedEvent.Invoke();
        }
    }
}
