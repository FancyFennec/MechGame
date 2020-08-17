using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int MaxHealth { get; } = 200;
    public int CurrentHealth { get; private set; } = 100;

    [SerializeField] private PlayerMovementController PlayerMovementController;
    [SerializeField] private PlayerShootingController PlayerShootingController;
    [SerializeField] private RecoilController recoilController;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        recoilController.AddRecoil(new Vector2(5, 0));
        CurrentHealth = Mathf.FloorToInt(Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth));
        if (CurrentHealth == 0f)
        {
            PlayerMovementController.enabled = false;
            PlayerShootingController.enabled = false;
        }
    }
}
