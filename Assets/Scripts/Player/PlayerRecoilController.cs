using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoilController : MonoBehaviour
{
	public Vector2 Recoil { get; private set; } = Vector2.zero;
	private Vector2 TargetRecoil = Vector2.zero;

	private void Awake()
	{
		PlayerHealth.instance.PlayerDamageTakenEvent += AimPunch;
	}

	void LateUpdate()
	{
		ManageRecoil();
		Recoil = Vector2.Lerp(Recoil, TargetRecoil, 20f * Time.deltaTime);
	}

	private void ManageRecoil()
	{
		Vector2 recoilDirection = TargetRecoil.normalized;
		if (TargetRecoil.magnitude < 0.1f && !Input.GetMouseButton(0))
		{
			TargetRecoil = Vector2.zero;
		}
		else
		{
			TargetRecoil =  Vector2.Lerp(TargetRecoil, Vector2.zero, 5f * Time.deltaTime);
		}
	}

	public void AddRecoil(Vector2 recoilVector)
	{
		TargetRecoil += recoilVector;
	}

	private void AimPunch()
	{
		AddRecoil(new Vector2(5, 0));
	}

	public bool IsRecoilReset => TargetRecoil == Vector2.zero;
}
