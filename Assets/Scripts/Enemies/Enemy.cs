using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    public float AttackCooldown = 1f;
    [System.NonSerialized]
    public float AttackTimer = 0f;
    public float StoppedCooldown = 1f;
    [System.NonSerialized]
    public float stoppedTimer = 0f;
    [Header("Splatter Particle")]
    public GameObject splatter;
    [Header("Ray Casting Input")]
    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected Transform head;

    protected NavMeshAgent navMeshAgent;
    protected Vector3 targetDirection = Vector3.zero;

    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;

    public virtual void AttackPlayer() { }
    public virtual void SearchPlayer() { }
    public virtual void BackUp() { }
    public virtual void CheckIfPlayerLost() { }
    public virtual void LookForPlayer() { }
    public virtual void RotateTowardsPlayer() { }
    public virtual void UpdateState() { }
    public void TakeDamage(float damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth);
        if(CurrentHealth == 0f)
        {
            NextState = EnemyState.DEAD;
            Destroy(Instantiate(splatter, transform.position, Quaternion.LookRotation(Vector3.up)), 2f);
        }
    }

    public void RotateTowardsDestination()
    {
        Vector3 position = Vector3.Scale(targetDirection, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Lerp(
            Quaternion.LookRotation(position, Vector3.up),
            transform.rotation,
            0.95f);
    }

    protected void UpdateTargetDirection()
    {
        if (CurrentState.Equals(EnemyState.ATTACKING))
        {
            targetDirection = (player.position - head.position);
            targetDirection.Scale(new Vector3(1, 0, 1));
            targetDirection = targetDirection.normalized;
        } else
        {
            targetDirection = (navMeshAgent.destination - transform.position);
            targetDirection.Scale(new Vector3(1, 0, 1));
            targetDirection = targetDirection.normalized;
        }
    }

    protected bool IsAtDestination()
    {
        return navMeshAgent.remainingDistance < 1.0f;
    }

    protected bool IsPlayerVisible()
    {
        Vector3 playerDirection = (player.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > -0.3f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }
}
