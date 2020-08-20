using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HomingRocketEnemy : Enemy
{

    private GameObject rocket;

    public override void Start()
    {
        base.Start();
        targetDirection = playerTarget.position - head.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        rocket = Resources.Load<GameObject>("Projectiles/HomingRocket");
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
                    navMeshAgent.isStopped = true;
                    break;
                case EnemyState.MOVING:
                    navMeshAgent.isStopped = false;
                    Vector3 layerDirection = playerTarget.position - transform.position;
                    navMeshAgent.destination = transform.position + 5f * layerDirection.normalized;
                    break;
                case EnemyState.SEARCHING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = playerTarget.position;
                    break;
            }
            CurrentState = NextState;
        }
    }

    public override void AttackPlayer()
    {
        RotateTowardsPlayer();
        ShootAtPlayer();
        CheckIfPlayerLost();
    }

    public override void Move()
    {
        RotateTowardsDestination();
        if (IsAtDestination())
        {
            NextState = EnemyState.ATTACKING;
        }
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

    public override void CheckIfPlayerLost()
    {
        if (!IsPlayerVisible())
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

    public override void Stop()
    {
        if (!IsOnCooldown)
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    public override void RotateTowardsPlayer()
    {
        Vector3 position = Vector3.Scale(targetDirection, new Vector3(1,0,1));
        transform.rotation = Quaternion.Lerp(
            Quaternion.LookRotation(position, Vector3.up),
            transform.rotation,
            0.95f);
    }

    private void ShootAtPlayer()
    {
        if (!IsOnCooldown)
        {
            try
            {
				Vector3 rocketSpawnPosition = (transform.position + 2 * transform.up);
                Instantiate(
                rocket,
                rocketSpawnPosition,
                Quaternion.LookRotation(transform.up, transform.forward)
                );
			}
            catch (Exception)
            {
                Debug.Log("Can't shoot");
            }
            IsOnCooldown = true;
            NextState = EnemyState.MOVING;
        }
    }
}
