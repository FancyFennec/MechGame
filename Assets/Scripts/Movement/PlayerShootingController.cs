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
    private ParticleSystem bulletTrails;
    private readonly List<Weapon> weapons = new List<Weapon> { 
        new Pistol(), 
        new AssaultRifle(),
        new RocketLauncher()
    };
    private Weapon currentWeapon = new Pistol();

    private List<int> numKeys = Enumerable.Range(1, 9).ToArray().ToList();
    private PlayerMovementController movementController;
    void Start() {
        movementController = GetComponent<PlayerMovementController>();
        blood = Resources.Load<GameObject>("Blood");
        rocket = Resources.Load<GameObject>("Rocket");
        bulletTrails = GetComponentInChildren<ParticleSystem>();
    }


    void Update()
    {
        if (IsShooting() && !currentWeapon.OnCooldown())
        {
            if (Weapon.Type.ROCKET.Equals(currentWeapon.type))
            {
                Instantiate(
                    rocket, 
                    transform.position + transform.forward * 1.5f, 
                    Quaternion.LookRotation(transform.forward, transform.up)
                    );
            } else
            {
                bulletTrails.Emit(1);
                if (IsEnemyHit(out RaycastHit hit))
                {
                    DamageEnemy(hit);

                    Vector3 incomingVec = (hit.point - transform.position).normalized;
                    Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
                    Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
                }
            }
            movementController.recoil += currentWeapon.Shoot();
        } else
        {
            Debug.Log("Ammo " + currentWeapon.ammo);
            Debug.Log("Clipsize " + currentWeapon.clipSize);
        }

        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("Reloading");
            currentWeapon.Reload();
        }
        weapons.ForEach(weapon => weapon.UpdateTimer(Time.deltaTime));
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

    private static bool IsEnemyHit(out RaycastHit hit)
    {
        return Physics.Raycast(
                        Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)),
                        out hit,
                        Mathf.Infinity,
                        LayerMask.GetMask("Enemy")
                        );
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
