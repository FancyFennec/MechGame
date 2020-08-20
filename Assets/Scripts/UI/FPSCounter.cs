using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        healthText.text = "fps: " + ((int) (1f / Time.deltaTime)).ToString();
    }
}
