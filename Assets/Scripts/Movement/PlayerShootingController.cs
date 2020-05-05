using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerShootingController : MonoBehaviour
{
    private GameObject blood;
    private GameObject rocket;
    private List<Weapon> weapons = new List<Weapon> { 
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
    }


    void Update()
    {
        if (IsShooting() && !currentWeapon.OnCooldown())
        {
            if (Weapon.Type.SINGLE_SHOT.Equals(currentWeapon.type))
            {
                Instantiate(
                    rocket, 
                    transform.position + transform.forward * 1.5f, 
                    Quaternion.LookRotation(transform.forward, transform.up)
                    );
            } else
            {
                if (IsEnemyHit(out RaycastHit hit))
                {
                    DamageEnemy(hit);

                    Vector3 incomingVec = (hit.point - transform.position).normalized;
                    Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
                    Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
                }
            }
            movementController.recoil += currentWeapon.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.ResetRecoil();
        }
        currentWeapon.UpdateTimer(Time.deltaTime);
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
            case Weapon.Type.SINGLE_SHOT:
                return Input.GetMouseButtonDown(0);
            default:
                return false;
        }
    }
}
