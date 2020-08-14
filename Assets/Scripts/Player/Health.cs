using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int MaxHealth = 100;
    [System.NonSerialized]
    public int CurrentHealth = 100;
    public TextMeshProUGUI HealthText;

    private PlayerMovementController PlayerMovementController;
    private PlayerShootingController PlayerShootingController;

    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthText.text = CurrentHealth.ToString();
        PlayerMovementController = transform.GetComponent<PlayerMovementController>();
        PlayerShootingController = transform.GetComponent<PlayerShootingController>();
    }

    public void TakeDamage(float damage)
    {
        PlayerMovementController.recoil += new Vector2(5, 0);
        CurrentHealth = Mathf.FloorToInt(Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth));
        HealthText.text = CurrentHealth.ToString();
        HealthText.color = Color.Lerp(Color.red, Color.green, CurrentHealth / MaxHealth);
        if (CurrentHealth == 0f)
        {
            PlayerMovementController.enabled = false;
            PlayerShootingController.enabled = false;
        }
    }
}
