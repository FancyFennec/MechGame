using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public List<GameObject> Canons;

    private GameObject enemySpawningRocket;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private readonly float cooldown = 8f;

    void Start()
    {
        enemySpawningRocket = Resources.Load<GameObject>("Projectiles/EnemySpawningRocket");
    }

    void Update()
    {
		if (!isOnCooldown)
		{
			StartCoroutine(ShootRockets());
		}
	}

	private IEnumerator ShootRockets()
	{
		foreach(GameObject canon in Canons)
		{
			RaycastHit hit = new RaycastHit();

			while (hit.normal != Vector3.up)
			{
				Vector3 randomDirection = new Vector3(
			Random.Range(-1f, 1f),
			Random.Range(-1f, -0.5f),
			Random.Range(-1f, 1f)
			).normalized;

				Physics.Raycast(canon.transform.position, randomDirection, out hit);
				if (hit.normal == Vector3.up)
				{
					Instantiate(enemySpawningRocket,
					canon.transform.position,
					Quaternion.LookRotation(randomDirection, Vector3.up));
					isOnCooldown = true;
				}
			}

		yield return new WaitForSeconds(1f);
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
