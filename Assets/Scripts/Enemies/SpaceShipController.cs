using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public List<GameObject> Canons;

    private GameObject enemySpawningRocket;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private readonly float cooldown = 2f;

    void Start()
    {
        enemySpawningRocket = Resources.Load<GameObject>("Projectiles/EnemySpawningRocket");
    }

    void Update()
    {
		if (!isOnCooldown)
		{
			Vector3 randomDirection = new Vector3(
				Random.Range(-1f, 1f), 
				Random.Range(-1f, -0.3f), 
				Random.Range(-1f, 1f)
				).normalized;

			/*bool blub = Physics.Raycast(
				Canons[0].transform.position,
				randomDirection,
				out RaycastHit hit
				);

			if(blub)
			{
				string name1 = hit.transform.gameObject.name;
			}

			bool hasHitDefaultLayer = Physics.Raycast(
				Canons[0].transform.position,
				randomDirection,
				~LayerMask.NameToLayer("Enemy")
				);*/

			Debug.Log("Shooting rocket");
			Instantiate(enemySpawningRocket,
				Canons[0].transform.position,
				Quaternion.LookRotation(randomDirection, Vector3.up));
			isOnCooldown = true;
		}
    }

    void LateUpdate()
	{
		UpdateCooldown();
	}

	private void UpdateCooldown()
	{
		if (isOnCooldown)
		{
			cooldownTimer += Time.deltaTime;
			if (cooldownTimer > cooldown)
			{
				isOnCooldown = false;
				cooldownTimer = 0f;

			}
		}
	}
}
