using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerRecoilController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings movementSettings;
    private PlayerRecoilController recoilController;
    private CharacterController characterController;

    private Transform cameraTransform;

    private float yClamp = 0f;
    private float xClamp = 0f;

    Vector3 movementVector = Vector3.zero;
    private Vector3 slidingDirection = Vector3.zero;

    bool isJumping = false;
    float jumpMomentum = 0f;

    private float floatingTime = 0f;
    private readonly float floatingThreshold = 0.2f;

    private bool isBoosting = false;
    public float BoostingTime { get; private set; } = 0f;
    public float BoostingThreshold { get; } = 2.0f;
    public bool isBoostingOnCooldown = false;
    private readonly float boostingCooldown = 4.0f;
    public bool IsBoostingOnCooldown
    {
        get { return isBoostingOnCooldown; }
        set
        {
            isBoostingOnCooldown = value;
            if (value)
            {
                StartCoroutine(RunAfterCooldown(boostingCooldown, () => isBoostingOnCooldown = false));
            }
        }
    }

    private bool isSliding = false;
    private float slidingTime = 0f;
    private readonly float slidingThreshold = 0.15f;
    private bool isSlidingOnCooldown  = false;
    private readonly float slidingCooldown = 1.5f;

    public bool IsSlidingOnCooldown
    {
        get { return isSlidingOnCooldown; }
        set
        {
            isSlidingOnCooldown = value;
            if (value)
            {
                StartCoroutine(RunAfterCooldown(slidingCooldown, () => isSlidingOnCooldown = false));
            }
        }
    }

    private void Awake()
	{
        cameraTransform = Camera.main.transform;
        recoilController = GetComponent<PlayerRecoilController>();
        characterController = GetComponent<CharacterController>();
        PlayerHealth.instance.PlayerDiedEvent += () => enabled = false;
    }

    void Update()
	{
        SetMovementVector();

        if (DoesPlayerWantToJump())
		{
			isJumping = true;
			jumpMomentum = movementSettings.JumpMomentum;
		}

        if(!isSliding && !IsSlidingOnCooldown)
		{
            if (DoesPlayerWantToSlide())
            {
                slidingDirection = movementVector;
                isSliding = true;
            }
        }

		if (Input.GetKeyDown(KeyCode.Space) && floatingTime < floatingThreshold)
		{
			isJumping = true;
			jumpMomentum = movementSettings.JumpMomentum;
		}

		isBoosting = Input.GetKey(KeyCode.Space) && !IsBoostingOnCooldown && floatingTime > floatingThreshold;

		RotateCamera();
		MovePlayer();
	}

    void LateUpdate()
    {
        ManageJumps();
        ManageBoosts();
        ManageSliding();
    }

    private bool DoesPlayerWantToSlide()
	{
		return movementVector != Vector3.zero && Input.GetKeyDown(KeyCode.LeftShift);
	}

	private bool DoesPlayerWantToJump()
	{
		return Input.GetKeyDown(KeyCode.Space) && floatingTime < floatingThreshold;
	}

	private void SetMovementVector()
	{
		movementVector = Vector3.zero;
		float hInput = Input.GetAxisRaw(movementSettings.HorizontalInput);
		float vInput = Input.GetAxisRaw(movementSettings.VerticalInput);
		movementVector += (transform.forward * vInput + transform.right * hInput).normalized;
	}

	private void ManageSliding()
	{
		if (isSliding)
		{
			if (slidingTime < slidingThreshold)
			{
				slidingTime += Time.deltaTime;
                floatingTime = 0f;
            }
			else
			{
                slidingDirection = Vector3.zero;
                slidingTime = 0f;
				isSliding = false;
				IsSlidingOnCooldown = true;
			}
		}
	}

	void RotateCamera()
    {
        // mouse x movement rotates around around Y-axis
        float mouseX = Input.GetAxis(movementSettings.MouseXInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);
        // mouse y movement rotates around around X-axis
        float mouseY = Input.GetAxis(movementSettings.MouseYInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);

		Vector3 cameraRotation = cameraTransform.eulerAngles;
        yClamp = Mathf.Clamp(yClamp + mouseY, movementSettings.MinAngle, movementSettings.MaxAngle);
        float newXAxisRotation = Mathf.Clamp(yClamp + recoilController.Recoil.x, movementSettings.MinAngle, movementSettings.MaxAngle);
        cameraRotation.x = -newXAxisRotation;
		cameraTransform.eulerAngles = cameraRotation;

        Vector3 bodyRotation = transform.eulerAngles;
        xClamp += mouseX;
        float newYAxisRotation = xClamp + recoilController.Recoil.y;
        bodyRotation.y = newYAxisRotation;
        transform.eulerAngles = bodyRotation;
    }

    void MovePlayer()
    {
		if (isSliding)
		{
            movementVector = slidingDirection * 100f;
		} else
		{
            movementVector *= movementSettings.MovementSpeed;

            movementVector += Vector3.up * jumpMomentum;

            if (isBoosting)
            {
                jumpMomentum -= movementSettings.Gravity * 0.2f * Time.smoothDeltaTime;
            }
            else
            {
                jumpMomentum -= movementSettings.Gravity * Time.smoothDeltaTime;
            }
        }

        characterController.Move(movementVector * Time.smoothDeltaTime);
    }
    private void ManageJumps()
    {
        if (characterController.isGrounded)
        {
            floatingTime = 0f;
            jumpMomentum = -0.1f;
        }
        else
        {
			if (!isSliding)// Don't update the floating time when sliding, then you can jump after sliding
			{
                floatingTime += Time.deltaTime;
            }
        }
        if (isJumping)
        {
            isJumping = false;
        }
    }

    private void ManageBoosts()
    {
        if (isBoosting)
        {
            BoostingTime += Time.deltaTime;
        }
        else
        {
            BoostingTime = Mathf.Clamp(BoostingTime - Time.deltaTime * 0.5f, 0f, BoostingThreshold);
        }

        if (!IsBoostingOnCooldown)
        {
            if (BoostingTime >= BoostingThreshold)
            {
                IsBoostingOnCooldown = true;
            }
        }
    }

    private IEnumerator RunAfterCooldown(float cooldown, Func<bool> fun)
    {
        yield return new WaitForSeconds(cooldown);
        fun();
    }
}
