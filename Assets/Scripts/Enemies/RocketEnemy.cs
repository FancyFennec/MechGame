﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RocketEnemy : Enemy
{

    private GameObject rocket;

    void Start()
    {
        targetDirection = player.position - head.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        rocket = Resources.Load<GameObject>("EnemyRocket");
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
                    if(!isOnCooldown)
					{
                        isOnCooldown = true;
                        navMeshAgent.isStopped = true;
                    } else
					{
                        NextState = CurrentState;
					}
                    break;
                case EnemyState.SEARCHING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = player.position;
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
        if (!isOnCooldown)
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

    public override void BackUp()
    {
        RotateTowardsDestination();
        if (IsAtDestination())
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    private void ShootAtPlayer()
    {
        if (!isOnCooldown)
        {
            try
            {
                Instantiate(
                rocket,
                transform.position + transform.up * 1.5f,
                Quaternion.LookRotation(transform.up, transform.forward)
                );
            }
            catch (Exception)
            {
                Debug.Log("Can't cause damage");
            }
            isOnCooldown = true;
            NextState = EnemyState.STOPPED;
        }
    }
}