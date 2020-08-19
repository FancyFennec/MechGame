using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayerShootingController : MonoBehaviour
{
	[SerializeField] private PlayerMovementController movementController;
	[SerializeField] private PlayerWeaponController weaponController;

	private const float zoomFactor = 0.5f;
	private float standardFov;
	private float zoomedFov;

	private readonly PlayerShotSignal shootSignal = new PlayerShotSignal();
    void Start()
	{
		SubscribeToShootSignal();
		standardFov = Camera.main.fieldOfView;
		zoomedFov = standardFov * zoomFactor;
	}

	void Update()
    {
        if (weaponController.DoesPlayerWantToShoot() && weaponController.CanWeaponFire)
		{
			shootSignal.Emmit();
			weaponController.Fire();
		}
		if (weaponController.CanWeaponZoom && Input.GetMouseButton(1))
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

    private void SubscribeToShootSignal()
    {
        FindObjectsOfType<Enemy>().ToList()
			.ForEach(enemy => shootSignal.Subscribe(enemy));
    }
}
