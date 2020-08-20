using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy: Enemy
{

    private GameObject explosion;
    
    private float playerNotSeenSinceTimer = 0f;

    public override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        explosion = Resources.Load<GameObject>("BigExplosionEffect");
        targetDirection = Vector3.forward;
    }

    public override void UpdateState()
    {
        if (CurrentState != NextState)
        {
            switch (NextState)
            {
                case EnemyState.DEAD:
                    navMeshAgent.isStopped = true;
                    Destroy(this.gameObject);
                    break;
                case EnemyState.IDLE:
                    navMeshAgent.isStopped = true;
                    break;
                case EnemyState.STOPPED:
                    navMeshAgent.isStopped = true;
                    break;
                case EnemyState.ATTACKING:
                    IsOnCooldown = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = playerTarget.position;
                    break;
                case EnemyState.SEARCHING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = playerTarget.position;
                    break;
                case EnemyState.MOVING:
                    navMeshAgent.isStopped = false;
                    Vector3 backupDirection = new Vector3(
                        UnityEngine.Random.Range(-1f, 1f), 
                        0, 
                        UnityEngine.Random.Range(-1f, 1f));
                    navMeshAgent.destination = transform.position + 20f * backupDirection.normalized;
                    break;
            }
            CurrentState = NextState;
        }
        playerNotSeenSinceTimer = IsPlayerVisible() ? 0f : playerNotSeenSinceTimer + Time.deltaTime;
    }

    public override void AttackPlayer()
    {
        RotateTowardsDestination();
        SetPlayerAsNavigationTarget();
        CheckIfPlayerLost();
    }

    public override void SearchPlayer()
    {
        if (IsPlayerVisible()) {
            NextState = EnemyState.ATTACKING;
        }
        else
        {
            if ((transform.position - navMeshAgent.destination).sqrMagnitude < 0.1f)
            {
                NextState = EnemyState.IDLE;
            }
        }
    }

    public override void Move()
    {
        RotateTowardsDestination();
        if (IsAtDestination())
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    public override void CheckIfPlayerLost()
    {
        if (playerNotSeenSinceTimer > 3f)
        {
            NextState = EnemyState.SEARCHING;
        }
    }

    public override void LookForPlayer()
    {
        if (IsPlayerVisible())
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    private void SetPlayerAsNavigationTarget()
    {
        navMeshAgent.destination = playerTarget.position;
    }

	private void OnTriggerEnter(Collider collision)
	{
        if (collision.gameObject.name == "Player")
        {
            if (!IsOnCooldown)
            {
                DamagePlayer();
            }
        }
    }

	private void DamagePlayer()
	{
        playerTarget.GetComponentInParent<PlayerHealth>().TakeDamage(40);
        Destroy(Instantiate(this.explosion, transform.position, Quaternion.identity), 3f);
        IsOnCooldown = true;
		NextState = EnemyState.MOVING;
	}
}
