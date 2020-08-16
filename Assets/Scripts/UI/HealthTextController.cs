using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthTextController : MonoBehaviour
{
    [SerializeField]
    private Health health;
    private TextMeshProUGUI healthText;
    private Color orange = new Color(1f, 0.5f, 0f);
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        healthText.text = health.CurrentHealth.ToString();
        healthText.color = Color.white;
    }

    void Update()
    {
        healthText.text = health.CurrentHealth.ToString();

        float value = (float)(float)health.CurrentHealth / health.MaxHealth;
        Color white_orange = Color.Lerp(orange, Color.white, 2 * value - 1f);
        healthText.color = Color.Lerp(Color.red, white_orange, 2 * value);
    }
}
