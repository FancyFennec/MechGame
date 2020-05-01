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
        SEARCHING,
        ATTACKING
    }

    [System.NonSerialized]
    public readonly float AttackCooldown = 2f;
    [System.NonSerialized]
    public float attackTimer = 0f;
    [System.NonSerialized]
    public readonly float StoppedCooldown = 1f;
    [System.NonSerialized]
    public float stoppedTimer = 0f;
    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;

    public virtual void AttackPlayer() { }
    public virtual void SearchPlayer() { }
    public virtual void CheckIfPlayerVisible() { }
    public virtual void LookForPlayer() { }
    public virtual void RotateTowardsPlayer() { }
    public virtual bool IsPlayerVisible() { return true; }
    public virtual void UpdateState() { }
}
