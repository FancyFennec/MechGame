using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Remoting.Messaging;

public class RecoilController : MonoBehaviour
{
	public Vector2 recoil { get; private set; } = Vector2.zero;

    void Start()
    {
        
    }

	void LateUpdate()
	{
		ManageRecoil();
	}

	private void ManageRecoil()
	{
		Vector2 recoilDirection = recoil.normalized;
		if (recoil.magnitude < 0.1f && !Input.GetMouseButton(0))
		{
			recoil = Vector2.zero;
		}
		else
		{
			recoil = new Vector2(
				Mathf.Clamp(recoil.x - recoilDirection.x * Time.deltaTime * 20f, -90f, 90f),
				Mathf.Clamp(recoil.y - recoilDirection.y * Time.deltaTime * 20f, -360f, 360f));
		}
	}

	public void AddRecoil(Vector2 recoilVector)
	{
		recoil += recoilVector;
	}

	public bool IsRecoilReset => recoil == Vector2.zero;
}
