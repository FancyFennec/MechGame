using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Remoting.Messaging;

public class PlayerRecoilController : MonoBehaviour
{
	public Vector2 Recoil { get; private set; } = Vector2.zero;

	private void Awake()
	{
		PlayerHealth.instance.PlayerDamageTakenEvent += AimPunch;
	}

	void LateUpdate()
	{
		ManageRecoil();
	}

	private void ManageRecoil()
	{
		Vector2 recoilDirection = Recoil.normalized;
		if (Recoil.magnitude < 0.1f && !Input.GetMouseButton(0))
		{
			Recoil = Vector2.zero;
		}
		else
		{
			Recoil = new Vector2(
				Mathf.Clamp(Recoil.x - recoilDirection.x * Time.deltaTime * 20f, -90f, 90f),
				Mathf.Clamp(Recoil.y - recoilDirection.y * Time.deltaTime * 20f, -360f, 360f));
		}
	}

	public void AddRecoil(Vector2 recoilVector)
	{
		Recoil += recoilVector;
	}

	private void AimPunch()
	{
		AddRecoil(new Vector2(5, 0));
	}

	public bool IsRecoilReset => Recoil == Vector2.zero;
}
