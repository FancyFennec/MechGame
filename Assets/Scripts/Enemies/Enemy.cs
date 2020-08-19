using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
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
    [Header("Health")]
    public int MaxHealth = 100;
    [System.NonSerialized]
    public float CurrentHealth = 100;

    [Header("Cooldowns")]
    public float Cooldown = 1f;
    [System.NonSerialized]
    public float CooldownTimer = 0f;
    protected bool isOnCooldown = false;

    [Header("Splatter Particle")]
    public GameObject splatter;

    protected Transform head;
    protected Transform player;

    protected NavMeshAgent navMeshAgent;
    protected Vector3 targetDirection = Vector3.zero;

    public EnemyState NextState { get; set; } = EnemyState.IDLE;
    public EnemyState CurrentState { get; set; } = EnemyState.IDLE;

    public virtual void AttackPlayer() { }
    public virtual void SearchPlayer() { }
    public virtual void Move() { }
    public virtual void Stop() { }
    public virtual void CheckIfPlayerLost() { }
    public virtual void LookForPlayer() { }
    public virtual void RotateTowardsPlayer() { }
    public virtual void UpdateState() { }

    public virtual void Start()
    {
        Health.instance.PlayerDiedEvent += () => NextState = EnemyState.STOPPED;
        PlayerShootingController.instance.ShotEvent += StartAttacking;
        player = Camera.main.transform;
        head = transform.Find("Head");
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
        UpdateCooldownTimer();
    }

    private void LateUpdate()
    {
        UpdateState();
    }

	private void OnDestroy()
	{
        PlayerShootingController.instance.ShotEvent -= StartAttacking;
    }

	public void TakeDamage(float damage) {

		if (CurrentState.Equals(EnemyState.IDLE))
		{
            NextState = EnemyState.ATTACKING;
		}

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth);
        if(CurrentHealth == 0f)
        {
            NextState = EnemyState.DEAD;
            Destroy(Instantiate(splatter, transform.position, Quaternion.LookRotation(Vector3.up)), 2f);
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

    protected void UpdateCooldownTimer()
    {
        if (isOnCooldown)
        {
            if (CooldownTimer < Cooldown)
            {
                CooldownTimer = Mathf.Clamp(CooldownTimer + Time.deltaTime, 0f, Cooldown);
            }
            else
            {
                CooldownTimer = 0f;
                isOnCooldown = false;
            }
        }
    }

    protected void UpdateTargetDirection()
    {
        if (CurrentState.Equals(EnemyState.ATTACKING))
        {
            targetDirection = (player.position - head.position);
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
        Vector3 playerDirection = (player.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > -0.3f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }

    protected bool IsAimingAtPlayer()
    {
        Vector3 playerDirection = (player.position - head.position).normalized;
        return Vector3.Dot(playerDirection, transform.forward) > 0.99f &&
                                Physics.Raycast(head.position, playerDirection, out RaycastHit hit)
                                && hit.transform.name == "Player";
    }

    public void StartAttacking()
    {
        if((transform.position - Camera.main.transform.position).magnitude < 50f)
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
    }
}
