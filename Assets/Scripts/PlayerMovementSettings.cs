using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Player/Settings", order = 1)]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Input Settings")]
    [SerializeField] private string mouseXInput = "Mouse X";
    [SerializeField] private string mouseYInput = "Mouse Y";
    [SerializeField] private string horizontalInput = "Horizontal";
    [SerializeField] private string verticalInput = "Vertical";

    [Header("Camera Settings")]
    [SerializeField] private readonly Vector2 minMaxAngles = new Vector2(-90, 90);

    [Header("Movement Settings")]
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private float movementSpeed = 10f;
    private const float jumpMomentum = 20f;
    private const float gravity = 50f;


    public string MouseXInput { get { return mouseXInput; } }
    public string MouseYInput { get { return mouseYInput; } }
    public string HorizontalInput { get { return horizontalInput; } }
    public string VerticalInput { get { return verticalInput; } }

    public float MouseSensitivity { get { return mouseSensitivity; } }
    public float MovementSpeed { get { return movementSpeed; } }

    public float MinAngle {get {return minMaxAngles.x; } }
    public float MaxAngle { get { return minMaxAngles.y; } }

    public float JumpMomentum { get { return jumpMomentum; } }
    public float Gravity { get { return gravity; } }
}
