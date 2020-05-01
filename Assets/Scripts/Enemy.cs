using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        DEAD,
        IDLE,
        SEARCHING,
        ATTACKING
    }

    [System.NonSerialized]
    public readonly float AttackCooldown = 2f;
    [System.NonSerialized]
    public float attackTimer = 0f;
    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;

    public abstract void AttackPlayer();
    public abstract void SearchPlayer();
    public abstract void CheckIfPlayerVisible();
    public abstract void LookForPlayer();
    public abstract void RotateTowardsPlayer();
    public abstract bool IsPlayerVisible();
    public abstract void UpdateState();
}
