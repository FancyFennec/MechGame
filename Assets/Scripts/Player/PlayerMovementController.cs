﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementSettings movementSettings;

    CharacterController characterController;
    Transform parentTransform;
    private float xClamp = 0f;
    private float yClamp = 0f;
    [System.NonSerialized]
    public Vector2 recoil = Vector2.zero;
    bool isJumping = false;
    float jumpMomentum = 0f;


    void Start()
    {
        Application.targetFrameRate = 120;

        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponentInParent<CharacterController>();
        parentTransform = characterController.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
        {
            isJumping = true;
        }
        RotateCamera();
        MovePlayer();
    }

    void LateUpdate()
    {
        Vector2 recoilDirection = recoil.normalized;
        if(recoil.magnitude < 0.01f)
        {
            recoil = Vector2.zero;
        } else
        {
            recoil.x = Mathf.Clamp(recoil.x - recoilDirection.x * Time.deltaTime * 20f, 0f, 20f);
            recoil.y = Mathf.Clamp(recoil.y - recoilDirection.y * Time.deltaTime * 20f, -360f, 360f);
        }
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis(movementSettings.MouseXInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);
        float mouseY = Input.GetAxis(movementSettings.MouseYInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);

        Vector3 eulerRotation = transform.eulerAngles;
        xClamp = Mathf.Clamp(xClamp + mouseY, movementSettings.MinAngle, movementSettings.MaxAngle);
        float newXRotation = Mathf.Clamp(xClamp + recoil.x, movementSettings.MinAngle, movementSettings.MaxAngle);
        eulerRotation.x = -newXRotation;
        transform.eulerAngles = eulerRotation;

        Vector3 parentEulerRotation = parentTransform.eulerAngles;
        yClamp += mouseX;
        float newYRotation = yClamp + recoil.y;
        parentEulerRotation.y = newYRotation;
        parentTransform.eulerAngles = parentEulerRotation;
    }

    void MovePlayer()
    {
        Vector3 movementVector = Vector3.zero;

        float hInput = Input.GetAxisRaw(movementSettings.HorizontalInput);
        float vInput = Input.GetAxisRaw(movementSettings.VerticalInput);

        movementVector += (parentTransform.forward * vInput + parentTransform.right * hInput).normalized;
        movementVector *= (Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f) * movementSettings.MovementSpeed;

        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
                jumpMomentum = movementSettings.JumpMomentum;
            } else
            {
                jumpMomentum = 0f;
            }
        }
        else
        {
            movementVector += Vector3.up * jumpMomentum;
            jumpMomentum -= movementSettings.Gravity * Time.smoothDeltaTime;
        }
        
        characterController.Move(movementVector * Time.smoothDeltaTime);
    }
}