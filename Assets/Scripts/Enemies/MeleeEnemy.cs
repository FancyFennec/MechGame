using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy: Enemy
{

    [Header("Ray Casting Input")]
    [SerializeField]
    private Transform player;
    public Transform head;
    private NavMeshAgent navMeshAgent;
    private List<ParticleSystem> explosions;

    private Vector3 targetDirection = Vector3.zero;
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
        targetDirection = (navMeshAgent.destination - head.position).normalized;
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
    }

    private void LateUpdate()
    {
        UpdateState();
    }

    public override void UpdateState()
    {
        if (CurrentState != NextState)
        {
            Debug.Log("Changing State to: " + NextState);
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

        if(attackTimer != 0f) attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0f, AttackCooldown);
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

    public void BackUp()
    {
        RotateTowardsDestination();
        if (IsAtDestination())
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    private bool HasReachedDestination()
    {
        return navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
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

    public void RotateTowardsDestination()
    {
        Vector3 position = Vector3.Scale(targetDirection, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Lerp(
            Quaternion.LookRotation(position, Vector3.up),
            transform.rotation,
            0.95f);
    }

    private void PunchPlayer()
    {
        navMeshAgent.destination = player.position;
        if (IsAtDestination())
        {
            if (attackTimer == 0)
            {
                try
                {
                    Debug.Log("Punching player");
                    player.parent.GetComponentInChildren<Health>().TakeDamage(20);
                    explosions.ForEach(expl => expl.Play());
                }
                catch (Exception)
                {
                    Debug.Log("Can't cause damage");
                }
                attackTimer = AttackCooldown;
                NextState = EnemyState.BACKUP;
            }
        }
    }

    private bool IsAtDestination()
    {
        return navMeshAgent.remainingDistance < 1.0f;
    }

    public override bool IsPlayerVisible()
    {
        Vector3 playerDirection = (player.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > -0.3f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }
}
