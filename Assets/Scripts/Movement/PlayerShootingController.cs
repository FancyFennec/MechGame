using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class PlayerShootingController : MonoBehaviour
{
    public TextMeshProUGUI AmmoCountText;

    private GameObject rocket;
    private GameObject bullet;
    private readonly List<Weapon> weapons = new List<Weapon> { 
        new Pistol(), 
        new AssaultRifle(),
        new RocketLauncher(),
        new AutomaticRocketLauncher()
    };
    private Weapon currentWeapon;

    private readonly List<String> numKeys = new List<string>(){"1", "2", "3", "4", "5", "6", "7", "8", "9"};
    public PlayerMovementController movementController;

    PlayerShotSignal shootSignal = new PlayerShotSignal();
    void Start()
	{
		SubscribeToShootSignal();
		currentWeapon = weapons[0];
		movementController = GetComponent<PlayerMovementController>();
		rocket = Resources.Load<GameObject>("Rocket");
        bullet = Resources.Load<GameObject>("Bullet");
        rocket.layer = this.gameObject.layer;
	}

	void Update()
    {
        if (DoesPlayerWantToShoot() && currentWeapon.CanWeaponFire())
		{
			shootSignal.Emmit();
			SpawnProjectile();
			AddRecoil();
		}
		if (Input.GetKey(KeyCode.R))
        {
            currentWeapon.Reload();
        }
        currentWeapon.UpdateCooldownTimer(Time.deltaTime);
    }

	private void AddRecoil()
	{
		movementController.recoil += currentWeapon.Shoot();
	}

	private void SpawnProjectile()
	{
		if (Projectile.ProjectileType.EXPLOSIVE.Equals(currentWeapon.projectileType))
		{
			Instantiate(
				rocket,
				transform.position + transform.forward * 1.5f,
				Quaternion.LookRotation(transform.forward, transform.up)
				);
		}
		else
		{
			Instantiate(
				bullet,
				transform.position + transform.forward * 1.5f,
				Quaternion.LookRotation(transform.forward, transform.up)
				);
		}
	}

	public void LateUpdate()
	{
		if (movementController.recoil.sqrMagnitude < 0.01f )
		{
			currentWeapon.ResetRecoil();
		}
		ChangeWeaponOnKeyPress();
	}

	private void ChangeWeaponOnKeyPress()
	{
		numKeys.ForEach((i) =>
		{
			int index = numKeys.IndexOf(i);
			if (Input.GetKeyDown(i) && index < weapons.Count)
			{
				currentWeapon = weapons[index];
			}
		});
		AmmoCountText.text = currentWeapon.ammo.ToString();
	}

	private bool DoesPlayerWantToShoot()
    {
        switch (currentWeapon.weaponType)
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
