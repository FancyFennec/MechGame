using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCountTextController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController playerWeaponController;

    private TextMeshProUGUI ammoCountText;
    private Color orange = new Color(1f, 0.5f, 0f);

    void Start()
    {
        ammoCountText = GetComponent<TextMeshProUGUI>();
        ammoCountText.color = Color.white;
    }

    void Update()
    {
        ammoCountText.text = playerWeaponController.Ammo.ToString();

		float value = (float)playerWeaponController.Ammo / playerWeaponController.ClipSize;
		Color white_orange = Color.Lerp(orange, Color.white, 2 * value - 1f);
		ammoCountText.color = Color.Lerp(Color.red, white_orange, 2 * value);
    }
}
