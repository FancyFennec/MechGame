using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [Header("Health")]
    public int MaxHealth = 100;
    [System.NonSerialized]
    public float CurrentHealth = 100;

    public event Action EnemyDiedEvent;
    public event Action EnemyDamageTakenEvent;

    [Header("Blood Splatter")]
    public GameObject splatter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        EnemyDamageTakenEvent.Invoke();
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth);
        if (CurrentHealth == 0f)
        {
            EnemyDiedEvent.Invoke();
            Destroy(Instantiate(splatter, transform.position, Quaternion.LookRotation(Vector3.up)), 2f);
        }
    }
}
