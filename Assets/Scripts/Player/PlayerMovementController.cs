using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovementSettings))]
[RequireComponent(typeof(PlayerRecoilController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementSettings movementSettings;
    private PlayerRecoilController recoilController;

    CharacterController characterController;
    private float yClamp = 0f;
    private float xClamp = 0f;
    bool isJumping = false;
    float jumpMomentum = 0f;
    private float floatingTime = 0f;
    private const float floatingThreshold = 0.2f;
    bool isBoosting = false;
    public float BoostingTime { get; private set; } = 0f;
    public float BoostingThreshold { get; } = 2.0f;
    public bool IsBoostingOnCooldown { get; private set; } = false;
    private float boostingCooldownTimer = 0f;
    private const float boostingCooldown = 4.0f;


	private void Awake()
	{
        recoilController = GetComponent<PlayerRecoilController>();
        PlayerHealth.instance.PlayerDiedEvent += () => enabled = false;
    }
	void Start()
    {
        Application.targetFrameRate = 120;

        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
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
	}

	void RotateCamera()
    {
        // mouse x movement rotates around around Y-axis
        float mouseX = Input.GetAxis(movementSettings.MouseXInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);
        // mouse y movement rotates around around X-axis
        float mouseY = Input.GetAxis(movementSettings.MouseYInput) * (movementSettings.MouseSensitivity * Time.smoothDeltaTime);

        Vector3 cameraRotation = Camera.main.transform.eulerAngles;
        yClamp = Mathf.Clamp(yClamp + mouseY, movementSettings.MinAngle, movementSettings.MaxAngle);
        float newXAxisRotation = Mathf.Clamp(yClamp + recoilController.Recoil.x, movementSettings.MinAngle, movementSettings.MaxAngle);
        cameraRotation.x = -newXAxisRotation;
        Camera.main.transform.eulerAngles = cameraRotation;

        Vector3 bodyRotation = transform.eulerAngles;
        xClamp += mouseX;
        float newYAxisRotation = xClamp + recoilController.Recoil.y;
        bodyRotation.y = newYAxisRotation;
        transform.eulerAngles = bodyRotation;
    }

    void MovePlayer()
    {
        Vector3 movementVector = Vector3.zero;

        float hInput = Input.GetAxisRaw(movementSettings.HorizontalInput);
        float vInput = Input.GetAxisRaw(movementSettings.VerticalInput);

        movementVector += (transform.forward * vInput + transform.right * hInput).normalized;
        movementVector *= (Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f) * movementSettings.MovementSpeed;

        movementVector += Vector3.up * jumpMomentum;

        if (isBoosting)
        {
            jumpMomentum -= movementSettings.Gravity * 0.2f * Time.smoothDeltaTime;
        } else
		{
            jumpMomentum -= movementSettings.Gravity * Time.smoothDeltaTime;
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
            floatingTime += Time.deltaTime;
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
        else
        {
            boostingCooldownTimer += Time.deltaTime;
            if (boostingCooldownTimer >= boostingCooldown)
            {
                IsBoostingOnCooldown = false;
                boostingCooldownTimer = 0f;
                BoostingTime = 0f;
            }
        }
    }
}
