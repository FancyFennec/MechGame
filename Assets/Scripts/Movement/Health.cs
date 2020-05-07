using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int MaxHealth = 100;
    [System.NonSerialized]
    public float CurrentHealth = 100;
    public TextMeshProUGUI HealthText;

    private PlayerMovementController PlayerMovementController;
    private PlayerShootingController PlayerShootingController;

    void Start()
    {
        HealthText.text = CurrentHealth.ToString();
        PlayerMovementController = transform.parent.GetComponentInChildren<PlayerMovementController>();
        PlayerShootingController = transform.parent.GetComponentInChildren<PlayerShootingController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        PlayerMovementController.recoil += new Vector2(15, 0);
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, MaxHealth);
        HealthText.text = CurrentHealth.ToString();
        HealthText.color = Color.Lerp(Color.red, Color.green, CurrentHealth / MaxHealth);
        if (CurrentHealth == 0f)
        {
            PlayerMovementController.enabled = false;
            PlayerShootingController.enabled = false;
        }
    }
}
