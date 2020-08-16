using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayerShootingController : MonoBehaviour
{
	private readonly List<String> weaponKeys = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
	private readonly List<Weapon> weapons = new List<Weapon> {
		new Pistol(),
		new AssaultRifle(),
		new RocketLauncher(),
		new AutomaticRocketLauncher(),
		new GrenadeLauncher()
	};
	private const float zoomFactor = 0.5f;
	private float standardFov;
	private float zoomedFov;
	public Weapon CurrentWeapon { get; private set; }
    private PlayerMovementController movementController;
	private readonly PlayerShotSignal shootSignal = new PlayerShotSignal();
    void Start()
	{
		SubscribeToShootSignal();
		movementController = GetComponent<PlayerMovementController>();
		CurrentWeapon = weapons[0];
		foreach (Weapon weapon in weapons)
		{
			weapon.projectile = Resources.Load<GameObject>(weapon.projectileAssetName);
		}
		standardFov = Camera.main.fieldOfView;
		zoomedFov = standardFov * zoomFactor;
	}

	void Update()
    {
        if (DoesPlayerWantToShoot() && CurrentWeapon.CanWeaponFire())
		{
			shootSignal.Emmit();
			SpawnProjectile();
			AddRecoil();
		}
		CurrentWeapon.UpdateCooldownTimer(Time.deltaTime);
		if (typeof(Pistol).IsInstanceOfType(CurrentWeapon) && Input.GetMouseButton(1))
		{
			Camera.main.fieldOfView = zoomedFov;
		} else
		{
			Camera.main.fieldOfView = standardFov;
		}
		if (Input.GetKey(KeyCode.R))
        {
            CurrentWeapon.Reload();
        }
    }

	private void AddRecoil()
	{
		movementController.recoil += CurrentWeapon.Shoot();
	}

	private void SpawnProjectile()
	{
		Instantiate(
				CurrentWeapon.projectile,
				Camera.main.transform.position + 
				Camera.main.transform.forward * 1.5f + 
				Camera.main.transform.right * (AlternateFire() ? -zoomFactor : zoomFactor),
				Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up)
				);
	}

	private bool AlternateFire()
	{
		return CurrentWeapon.ammo % 2 == 0;
	}

	public void LateUpdate()
	{
		if (movementController.recoil.sqrMagnitude < 0.01f )
		{
			CurrentWeapon.ResetRecoil();
		}
		ChangeWeaponOnKeyPress();
	}

	private void ChangeWeaponOnKeyPress()
	{
		weaponKeys.ForEach((i) =>
		{
			int index = weaponKeys.IndexOf(i);
			if (Input.GetKeyDown(i) && index < weapons.Count)
			{
				CurrentWeapon = weapons[index];
			}
		});
	}

	private bool DoesPlayerWantToShoot()
    {
		switch (CurrentWeapon.weaponType)
		{
			case Weapon.WeaponType.SEMI_AUTOMATIC:
				return Input.GetMouseButtonDown(0);
			case Weapon.WeaponType.AUTOMATIC:
				return Input.GetMouseButton(0);
			default:
				return false;
		}
	}

    private void SubscribeToShootSignal()
    {
        FindObjectsOfType<Enemy>().ToList()
			.ForEach(enemy => shootSignal.Subscribe(enemy));
    }
}
