using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerShootingController : MonoBehaviour
{
	private PlayerWeaponController weaponController;

	public static PlayerShootingController instance;

	private const float zoomFactor = 0.5f;
	private float standardFov;
	private float zoomedFov;

	public event Action PrimaryShotEvent;

	private void Awake()
	{
		instance = this;
		weaponController = GetComponent<PlayerWeaponController>();
		PlayerHealth.instance.PlayerDiedEvent += () => enabled = false;
	}

	void Start()
	{
		standardFov = Camera.main.fieldOfView;
		zoomedFov = standardFov * zoomFactor;
	}

	void Update()
    {
        if (weaponController.DoesPlayerWantToShootPrimary() && weaponController.CanPrimaryFire)
		{
			PrimaryShotEvent?.Invoke();
			weaponController.FirePrimary();
		}
		if (weaponController.DoesPlayerWantToShootSecondary() && weaponController.CanSecondaryFire)
		{
			//PrimaryShotEvent?.Invoke();
			weaponController.FireSecondary();
		}
		if (weaponController.CanPrimaryZoom && Input.GetMouseButton(2))
		{
			Camera.main.fieldOfView = zoomedFov;
		} else
		{
			Camera.main.fieldOfView = standardFov;
		}
		if (Input.GetKey(KeyCode.R))
        {
			weaponController.Reload();
        }
    }
}
