using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    public enum EnemyState
    {
        DEAD,
        IDLE,
        STOPPED,
        BACKUP,
        SEARCHING,
        ATTACKING
    }
    [Header("Health")]
    public int MaxHealth = 100;
    [System.NonSerialized]
    public float CurrentHealth = 100;
    [Header("Cooldowns")]
    public float AttackCooldown = 2f;
    [System.NonSerialized]
    public float attackTimer = 0f;
    public float StoppedCooldown = 1f;
    [System.NonSerialized]
    public float stoppedTimer = 0f;
    [Header("Splatter Particle")]
    public GameObject splatter;

    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;

    public virtual void AttackPlayer() { }
    public virtual void SearchPlayer() { }
    public virtual void CheckIfPlayerVisible() { }
    public virtual void LookForPlayer() { }
    public virtual void RotateTowardsPlayer() { }
    public virtual bool IsPlayerVisible() { return true; }
    public virtual void UpdateState() { }
    public void TakeDamage(float damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth);
        if(CurrentHealth == 0f)
        {
            NextState = EnemyState.DEAD;
            Destroy(Instantiate(splatter, transform.position, Quaternion.LookRotation(Vector3.up)), 2f);
        }
    }
}
