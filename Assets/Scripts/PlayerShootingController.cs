using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShootingController : MonoBehaviour
{
    [Header("Blood Particle")]
    public GameObject blood;
    private Weapon weapon = new AssaultRifle();

    private PlayerMovementController movementController;
    void Start() {
        movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !weapon.OnCooldown())
        {
            if (Physics.Raycast(
                Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)),
                out RaycastHit hit,
                Mathf.Infinity,
                LayerMask.GetMask("Enemy")
                ))
            {
                Vector3 incomingVec = (hit.point - transform.position).normalized;
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                try
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(20);
                        //Debug.Log("Current Health: " + enemy.CurrentHealth);
                    }
                    else
                    {
                        Enemy parentEnemy = hit.transform.GetComponentInParent<Enemy>();
                        if (parentEnemy != null)
                        {
                            parentEnemy.TakeDamage(20);
                            //Debug.Log("Current Health: " + parentEnemy.CurrentHealth);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Something went wrong: " + e.Message);
                }
                Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
            }
            Vector2 recoil = weapon.Shoot();
            movementController.recoil += recoil;
        }
        if (!Input.GetMouseButton(0))
        {
            if (movementController.recoil.magnitude < 0.1f)
            {
                weapon.ResetRecoil();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.ResetRecoil();
        }
        weapon.UpdateTimer(Time.deltaTime);
    }
}
