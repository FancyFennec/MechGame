using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCountTextController : MonoBehaviour
{
    [SerializeField]
    private PlayerShootingController playerShootingController;
    private TextMeshProUGUI ammoCountText;

    void Start()
    {
        ammoCountText = GetComponent<TextMeshProUGUI>();
        ammoCountText.color = Color.green;
    }

    void Update()
    {
        ammoCountText.text = playerShootingController.currentWeapon.ammo.ToString();
        ammoCountText.color = Color.Lerp(Color.red, Color.green, (float)playerShootingController.currentWeapon.ammo / playerShootingController.currentWeapon.clipSize);
    }
}
