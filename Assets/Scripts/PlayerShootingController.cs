using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShootingController : MonoBehaviour
{
    [Header("Blood Particle")]
    public GameObject blood;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                    //hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 20f);
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(20);
                        Debug.Log("Current Health: " + enemy.CurrentHealth);
                    }
                    else
                    {
                        Enemy parentEnemy = hit.transform.GetComponentInParent<Enemy>();
                        if (parentEnemy != null)
                        {
                            parentEnemy.TakeDamage(20);
                            Debug.Log("Current Health: " + parentEnemy.CurrentHealth);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception thrown: " + e.Message);
                }
                Destroy(Instantiate(blood, hit.point, Quaternion.LookRotation(reflectVec)), 1f);
            }
        }
    }
}
