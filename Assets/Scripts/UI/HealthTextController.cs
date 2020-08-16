using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthTextController : MonoBehaviour
{
    [SerializeField]
    private Health health;
    private TextMeshProUGUI healthText;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        healthText.text = health.CurrentHealth.ToString();
        healthText.color = Color.green;
    }

    void Update()
    {
        healthText.text = health.CurrentHealth.ToString();
        healthText.color = Color.Lerp(Color.red, Color.green, (float) health.CurrentHealth / health.MaxHealth);
    }
}
