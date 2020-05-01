using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public enum EnemyState
    {
        IDLE,
        SEARCHING,
        ATTACKING
    }

    [SerializeField]
    private Transform target;
    public Transform head;
    private NavMeshAgent navMeshAgent;

    private readonly float fireCooldown = 2f;
    private float fireTimer = 0f;

    public EnemyState NextState { private get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { private get; set; } = EnemyState.IDLE;
    private Vector3 targetDirection = Vector3.zero;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetDirection = target.position - head.position;
    }

    void Update()
    {
        targetDirection = target.position - head.position;
        switch (CurrentState) {
            case EnemyState.IDLE:
                WaitForPlayer();
                break;
            case EnemyState.ATTACKING:
                RotateTowardsPlayer();
                ShootAtPlayer();
                CheckIfPlayerVisible();
                break;
            case EnemyState.SEARCHING:
                SearchPlayer();
                break;
        }
    }

    private void LateUpdate()
    {
        if (CurrentState != NextState)
        {
            switch (NextState)
            {
                case EnemyState.IDLE:
                    navMeshAgent.isStopped = false;
                    break;
                case EnemyState.ATTACKING:
                    navMeshAgent.isStopped = false;
                    break;
                case EnemyState.SEARCHING:
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = target.position;
                    break;
            }
            CurrentState = NextState;
        }
    }

    private void SearchPlayer()
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

    private void CheckIfPlayerVisible()
    {
        if (!IsPlayerVisible())
        {
            NextState = EnemyState.SEARCHING;
        }
    }

    private void WaitForPlayer()
    {
        if (IsPlayerVisible())
        {
            NextState = EnemyState.ATTACKING;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 position = Vector3.Scale(targetDirection, new Vector3(1,0,1));
        transform.rotation = Quaternion.Lerp(
            Quaternion.LookRotation(position, Vector3.up),
            transform.rotation,
            0.95f);
    }

    private void ShootAtPlayer()
    {
        if(fireTimer == 0)
        {
            Debug.Log("Peng!");
            fireTimer = fireCooldown;
        } else
        {
            fireTimer = Mathf.Clamp(fireTimer - Time.deltaTime, 0f, fireCooldown);
        }
    }

    private bool IsPlayerVisible()
    {
        return Vector3.Dot(targetDirection, transform.forward) > 0 &&
                                Physics.Raycast(head.position, targetDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }
}
