using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class PlayerShootingController : MonoBehaviour
{
    public TextMeshProUGUI AmmoCountText;

    private GameObject blood;
    private GameObject rocket;
    private GameObject bullet;
    private readonly List<Weapon> weapons = new List<Weapon> { 
        new Pistol(), 
        new AssaultRifle(),
        new RocketLauncher()
    };
    private Weapon currentWeapon;

    private readonly List<int> numKeys = Enumerable.Range(1, 9).ToArray().ToList();
    public PlayerMovementController movementController;

    PlayerShotSignal shootSignal = new PlayerShotSignal();
    void Start()
	{
		subscribeToShootSignal();
		currentWeapon = weapons[0];
		movementController = GetComponent<PlayerMovementController>();
		blood = Resources.Load<GameObject>("Blood");
		rocket = Resources.Load<GameObject>("Rocket");
        bullet = Resources.Load<GameObject>("Bullet");
        rocket.layer = this.gameObject.layer;
	}

	private void subscribeToShootSignal()
	{
		FindObjectsOfType<Enemy>().ToList()
					.ForEach(enemy => shootSignal.Subscribe(enemy));
	}

	void Update()
    {
        if (IsShooting() && !currentWeapon.OnCooldown())
        {
            shootSignal.Emmit();
            if (Weapon.Type.ROCKET.Equals(currentWeapon.type))
            {
                Instantiate(
                    rocket, 
                    transform.position + transform.forward * 1.5f, 
                    Quaternion.LookRotation(transform.forward, transform.up)
                    );
            } else
            {
                Instantiate(
                    bullet,
                    transform.position + transform.forward * 1.5f,
                    Quaternion.LookRotation(transform.forward, transform.up)
                    );
            }
            movementController.recoil += currentWeapon.Shoot();
        }
        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("Reloading");
            currentWeapon.Reload();
        }
        weapons.ForEach(weapon => weapon.UpdateCooldownTimer(Time.deltaTime));
    }

    public void LateUpdate()
    {
        if (movementController.recoil == Vector2.zero)
        {
            currentWeapon.ResetRecoil();
        }

        numKeys.ForEach((i) =>
        {
            if (Input.GetKeyDown(i.ToString()) && --i < weapons.Count)
            {
                currentWeapon = weapons[i];
            }
        });
        AmmoCountText.text = currentWeapon.ammo.ToString();
    }

    private void DamageEnemy(RaycastHit hit)
    {
        try
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
            }
            else
            {
                Enemy parentEnemy = hit.transform.GetComponentInParent<Enemy>();
                if (parentEnemy != null)
                {
                    parentEnemy.TakeDamage(20);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Something went wrong: " + e.Message);
        }
    }

    private bool IsShooting()
    {
        switch (currentWeapon.type)
        {
            case Weapon.Type.SEMI_AUTOMATIC:
                return Input.GetMouseButtonDown(0);
            case Weapon.Type.AUTOMATIC:
                return Input.GetMouseButton(0);
            case Weapon.Type.ROCKET:
                return Input.GetMouseButtonDown(0);
            default:
                return false;
        }
    }
}
