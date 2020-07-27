using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy: Enemy
{

    private List<ParticleSystem> explosions;
    
    private float playerNotSeenSinceTimer = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetDirection = Vector3.forward;
        GameObject explosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform);
        explosion.transform.parent = transform;
        explosions = explosion.GetComponentsInChildren<ParticleSystem>().ToList();
        explosions.ForEach(expl => expl.Stop());
    }

    void Update()
    {
        UpdateCooldownTimer();
        UpdateTargetDirection();
        switch (CurrentState)
        {
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
    }

    private void LateUpdate()
    {
        UpdateState();
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
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = player.position;
                    break;
                case EnemyState.SEARCHING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = player.position;
                    break;
                case EnemyState.BACKUP:
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

        if(AttackTimer != 0f) AttackTimer = Mathf.Clamp(AttackTimer - Time.deltaTime, 0f, AttackCooldown);
        playerNotSeenSinceTimer = IsPlayerVisible() ? 0f : playerNotSeenSinceTimer + Time.deltaTime;
    }

    public override void AttackPlayer()
    {
        RotateTowardsDestination();
        PunchPlayer();
        CheckIfPlayerLost();
    }

    private void Stop()
    {
        if (stoppedTimer == StoppedCooldown)
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

    public override void BackUp()
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

    private void PunchPlayer()
    {
        navMeshAgent.destination = player.position;
        if (IsAtDestination() || (player.position - transform.position).magnitude < 1.5f)
        {
            if (!isOnCooldown)
            {
                try
                {
                    float damagefactor = 1.5f - navMeshAgent.remainingDistance;
                    player.parent.GetComponentInChildren<Health>().TakeDamage(40 * damagefactor);
                    explosions.ForEach(expl => expl.Play());
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
}
