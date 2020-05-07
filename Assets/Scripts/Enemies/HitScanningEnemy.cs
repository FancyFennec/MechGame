using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HitScanningEnemy: Enemy
{
    [Header("Ray Casting Input")]
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform head;
    public ParticleSystem bulletTrails;

    private NavMeshAgent navMeshAgent;
    private Vector3 targetDirection = Vector3.zero;

    void Start()
    {
        //bulletTrails = GetComponentInChildren<ParticleSystem>();
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
                    navMeshAgent.isStopped = true;
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

    private void ShootAtPlayer()
    {
        if(attackTimer == 0)
        {
            bulletTrails.Emit(1);
            try
            {
                if(UnityEngine.Random.Range(0, 10) > 3)
                {
                    Debug.Log("Shooting at player");
                    player.parent.GetComponentInChildren<Health>().TakeDamage(5);
                } else
                {
                    Debug.Log("Missed player");
                    //TODO: cast a bullet trail
                }
            }
            catch (Exception) {
                Debug.Log("Can't cause damage");
            }
            attackTimer = AttackCooldown;
            NextState = EnemyState.STOPPED;
        } else
        {
            attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0f, AttackCooldown);
        }
    }

    public override bool IsPlayerVisible()
    {
        return Vector3.Dot(targetDirection, transform.forward) > 0 &&
                                Physics.Raycast(head.position, targetDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }
}
