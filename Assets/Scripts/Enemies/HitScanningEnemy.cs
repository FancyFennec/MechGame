using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HitScanningEnemy: Enemy
{
    public ParticleSystem bulletTrails;

    void Start()
    {
        bulletTrails = GetComponentInChildren<ParticleSystem>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetDirection = player.position - head.position;
    }

    void Update()
    {
        UpdateTargetDirection();
        switch (CurrentState) {
            case EnemyState.DEAD:
                break;
            case EnemyState.IDLE:
                LookForPlayer();
                break;
            case EnemyState.STOPPED:
                Stop();
                break;
            case EnemyState.BACKUP:
                BackUp();
                break;
            case EnemyState.ATTACKING:
                AttackPlayer();
                break;
            case EnemyState.SEARCHING:
                SearchPlayer();
                break;
        }
        UpdateCooldownTimer();
    }

    private void LateUpdate()
    {
        UpdateState();
    }

    public override void UpdateState()
    {
        if (CurrentState != NextState)
        {
            Debug.Log("Switching State: " + NextState);
            switch (NextState)
            {
                case EnemyState.DEAD:
                    navMeshAgent.isStopped = true;
                    Destroy(this.gameObject);
                    break;
                case EnemyState.IDLE:
                    navMeshAgent.isStopped = true;
                    break;
                case EnemyState.ATTACKING:
                    navMeshAgent.isStopped = true;
                    break;
                case EnemyState.BACKUP:
                    navMeshAgent.isStopped = false;
                    Vector3 backupDirection = new Vector3(
                        UnityEngine.Random.Range(-1f, 1f),
                        0,
                        UnityEngine.Random.Range(-1f, 1f));
                    navMeshAgent.destination = transform.position + 10f * backupDirection.normalized;
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

    private void Stop()
    {
        if (stoppedTimer >= StoppedCooldown)
        {
            stoppedTimer = 0f;
            NextState = EnemyState.SEARCHING;
        }
        else
        {
            stoppedTimer = Mathf.Clamp(stoppedTimer + Time.deltaTime, 0f, StoppedCooldown);
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
        if (!isOnCooldown && IsAimingAtPlayer())
		{
            //TODO: shoot projectile
            //bulletTrails.Emit(1);
            try
            {
                if (UnityEngine.Random.Range(0, 10) > 3)
                {
                    player.parent.GetComponentInChildren<Health>().TakeDamage(5);
                }
                else
                {
                    //TODO: cast a bullet trail
                }
            }
            catch (Exception)
            {
                Debug.Log("Can't cause damage");
            }
            isOnCooldown = true;
            NextState = EnemyState.BACKUP;
        }
    }
}
