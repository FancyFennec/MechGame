using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField]
    public int MaxHealth { get; } = 200;
    public int CurrentHealth { get; private set; } = 100;

    private PlayerMovementController PlayerMovementController;
    private PlayerShootingController PlayerShootingController;

    void Start()
    {
        CurrentHealth = MaxHealth;
        PlayerMovementController = transform.GetComponent<PlayerMovementController>();
        PlayerShootingController = transform.GetComponent<PlayerShootingController>();
    }

    public void TakeDamage(float damage)
    {
        PlayerMovementController.recoil += new Vector2(5, 0);
        CurrentHealth = Mathf.FloorToInt(Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth));
        if (CurrentHealth == 0f)
        {
            PlayerMovementController.enabled = false;
            PlayerShootingController.enabled = false;
        }
    }
}
