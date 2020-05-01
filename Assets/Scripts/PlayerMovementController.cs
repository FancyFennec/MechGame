using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Input Settings")]
    public string MouseXInput;
    public string MouseYInput;
    public string HorizontalInput = "Horizontal";
    public string VerticalInput = "Vertical";

    [Header("Common Settings")]
    public float mouseSensitivity;
    public float movementSpeed;

    [Header("FPS Camera Settings")]
    public Vector3 FPS_CameraOffset;
    public Vector2 FPS_MinMaxAngles;

    [Header("Movement Settings")]
    public const float initialJumpMomentum = 20f;
    public const float gravity = 50f;

    CharacterController characterController;
    Transform FPSController;
    float xClamp;
    bool isJumping = false;
    float jumpMomentum = 0f;

    public GameObject blood;

    void Start()
    {
        Application.targetFrameRate = 120;

        Cursor.lockState = CursorLockMode.Locked;
        xClamp = 0;
        characterController = GetComponentInParent<CharacterController>();
        FPSController = characterController.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
        {
            isJumping = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(
                Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), 
                out RaycastHit hit,
                Mathf.Infinity,
                LayerMask.GetMask("Enemy")
                ))
            {
                Vector3 incomingVec = (hit.point - transform.position).normalized;
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                try
                {
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 20f);
                }
                catch(MissingComponentException e) { }
                Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
            }
        }
        RotateCamera();
        MovePlayer();
    }

    void RotateCamera()
    {
        Vector3 eulerRotation = transform.eulerAngles;
        float mouseX = Input.GetAxis(MouseXInput) * (mouseSensitivity * Time.smoothDeltaTime);
        float mouseY = Input.GetAxis(MouseYInput) * (mouseSensitivity * Time.smoothDeltaTime);

        xClamp = Mathf.Clamp(xClamp + mouseY, FPS_MinMaxAngles.x, FPS_MinMaxAngles.y);

        eulerRotation.x = -xClamp;
        transform.eulerAngles = eulerRotation;
        FPSController.Rotate(Vector3.up * mouseX);
    }

    void MovePlayer()
    {
        Vector3 movementVector = Vector3.zero;

        float hInput = Input.GetAxisRaw(HorizontalInput);
        float vInput = Input.GetAxisRaw(VerticalInput);

        movementVector += (FPSController.forward * vInput + FPSController.right * hInput).normalized;
        movementVector *= (Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f) * movementSpeed;

        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
                jumpMomentum = initialJumpMomentum;
            } else
            {
                jumpMomentum = 0f;
            }
        }
        else
        {
            movementVector += Vector3.up * jumpMomentum;
            jumpMomentum -= gravity * Time.smoothDeltaTime;
        }
        
        characterController.Move(movementVector * Time.smoothDeltaTime);
    }
}
