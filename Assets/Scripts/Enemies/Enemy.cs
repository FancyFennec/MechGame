using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(CooldownController))]
[RequireComponent(typeof(AudioController))]
public abstract class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        DEAD,
        IDLE,
        STOPPED,
        MOVING,
        SEARCHING,
        ATTACKING
    }
    private EnemyHealth health;
    private CooldownController cooldownController;
    protected AudioController audioController;

    protected Transform head;
    protected Transform playerTarget;

    protected NavMeshAgent navMeshAgent;
    protected Vector3 targetDirection = Vector3.zero;

    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;
    public Boolean IsOnCooldown { get => cooldownController.IsOnCooldown; set => cooldownController.IsOnCooldown = value;}

    public virtual void AttackPlayer() { }
    public virtual void SearchPlayer() { }
    public virtual void Move() { }
    public virtual void Stop() { }
    public virtual void CheckIfPlayerLost() { }
    public virtual void LookForPlayer() { }
    public virtual void RotateTowardsPlayer() { }
    public virtual void UpdateState() { }

	private void Awake()
	{
        health = GetComponent<EnemyHealth>();
        cooldownController = GetComponent<CooldownController>();
        audioController = GetComponent<AudioController>();

        playerTarget = Camera.main.transform;
        head = transform.Find("Head");
    }

	public virtual void Start() {
        PlayerHealth.instance.PlayerDiedEvent += () => NextState = EnemyState.STOPPED;
        PlayerShootingController.instance.ShotEvent += StartAttackingIfShotWasHeard;

        health.EnemyDiedEvent += () => NextState = EnemyState.DEAD;
        health.EnemyDamageTakenEvent += StartAttacking;
    }

    void Update()
    {
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
            case EnemyState.MOVING:
                Move();
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

	private void OnDestroy()
	{
        PlayerShootingController.instance.ShotEvent -= StartAttackingIfShotWasHeard;
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
            targetDirection = (playerTarget.position - head.position);
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
        Vector3 playerDirection = (playerTarget.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > -0.3f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }

    protected bool IsAimingAtPlayer()
    {
        Vector3 playerDirection = (playerTarget.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > 0.99f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }

    public void StartAttacking()
    {
        switch (CurrentState)
        {
            case EnemyState.MOVING:
                break;
            case EnemyState.ATTACKING:
                break;
            default:
                NextState = EnemyState.ATTACKING;
                break;
        }
    }

    public void StartAttackingIfShotWasHeard()
    {
        if ((transform.position - Camera.main.transform.position).magnitude < 50f)
        {
            StartAttacking();
        }
    }
}
