using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponModification : MonoBehaviour
{
	PlayerWeaponController weaponController;
	readonly List<GameObject> primaryWeapons = new List<GameObject>();
	readonly List<GameObject> secondaryWeapons = new List<GameObject>();
	int currentPrimary;
	int currentSecondary;


	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		weaponController = GetWeaponController();
		currentPrimary = weaponController.PrimaryIndex;
		currentSecondary = weaponController.SecondaryIndex;

		Transform weapons = GameObject.Find("PrimaryWeapons").transform;
		for (int i = 0; i < weapons.childCount; i++)
		{
			primaryWeapons.Add(weapons.GetChild(i).gameObject);
		}

		weapons = GameObject.Find("SecondaryWeapons").transform;
		for (int i = 0; i < weapons.childCount; i++)
		{
			secondaryWeapons.Add(weapons.GetChild(i).gameObject);
		}
	}

	private PlayerWeaponController GetWeaponController()
	{
		Scene scene = SceneManager.GetSceneByName("MainScene");
		GameObject player = scene.GetRootGameObjects().ToList().Find(gameObject => gameObject.name == "Player");
		return player.GetComponent<PlayerWeaponController>();
	}

	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0) && IsMouseHoveringWeapon(out RaycastHit hit))
		{
			Rigidbody rb = hit.collider.gameObject.GetComponentInParent<Rigidbody>();
			int primaryIndex = primaryWeapons.IndexOf(rb.gameObject);
			int secondaryIndex = secondaryWeapons.IndexOf(rb.gameObject);
			currentPrimary = primaryIndex == -1 ? currentPrimary : primaryIndex;
			currentSecondary = secondaryIndex == -1 ? currentSecondary : secondaryIndex;
		}
	}

	private void OnDestroy()
	{
		weaponController.SwitchPrimaryWeapon(currentPrimary);
		weaponController.SwitchSecondaryWeapon(currentSecondary);
	}

	private bool IsMouseHoveringWeapon(out RaycastHit hit)
	{
		return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
	}
}
