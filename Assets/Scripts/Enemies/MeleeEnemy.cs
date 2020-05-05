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

    private Vector3 targetDirection = Vector3.zero;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetDirection = player.position - head.position;
    }

    void Update()
    {
        targetDirection = player.position - head.position;
        switch (CurrentState) {
            case EnemyState.DEAD:
                break;
            case EnemyState.IDLE:
                LookForPlayer();
                break;
            case EnemyState.STOPPED:
                Stop();
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
                case EnemyState.ATTACKING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = player.position;
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
        PunchPlayer();
        CheckIfPlayerVisible();
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

    public override void CheckIfPlayerVisible()
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

    private void PunchPlayer()
    {
        navMeshAgent.destination = player.position;
        if (attackTimer == 0)
        {
            if (IsInPunchingRange())
            {
                Debug.Log("Punch!");
                attackTimer = AttackCooldown;
                NextState = EnemyState.STOPPED;
            }
        }
        else
        {
            if (IsInPunchingRange())
            {
                Debug.Log("Stopping!");
                NextState = EnemyState.STOPPED;
            }
            attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0f, AttackCooldown);
        }
    }

    private bool IsInPunchingRange()
    {
        return (transform.position - navMeshAgent.destination).sqrMagnitude < 1.5f;
    }

    public override bool IsPlayerVisible()
    {
        return Vector3.Dot(targetDirection, transform.forward) > -0.3f &&
                                Physics.Raycast(head.position, targetDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }
}
