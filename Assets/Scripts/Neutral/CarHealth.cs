using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarHealth : MonoBehaviour, IHealth
{
	GameObject explosion;
	void Awake()
    {
		explosion = Resources.Load<GameObject>("SmallExplosionEffect");
	}

	public void TakeDamage(float damage)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			AddForcesToChildren(i);
		}
		CreateExplosion();
		Destroy(gameObject);
	}

	private void AddForcesToChildren(int i)
	{
		Vector3 randomDirection = Vector3.up + new Vector3(
						Random.Range(-1f, 1f),
						0,
						Random.Range(-1f, 1f));

		Transform child = transform.GetChild(i);
		child.parent = transform.parent;
		child.gameObject.AddComponent<Rigidbody>();
		child.gameObject.GetComponent<Rigidbody>().mass = 0.3f;
		child.gameObject.GetComponent<Rigidbody>().AddForce(randomDirection.normalized * 300f);
	}

	private void CreateExplosion()
	{
		GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
		float delay = (Camera.main.transform.position - transform.position).magnitude * 0.005f;
		explosionObject.GetComponent<AudioSource>().PlayDelayed(delay);
		Destroy(explosionObject, 2f);
	}
}
