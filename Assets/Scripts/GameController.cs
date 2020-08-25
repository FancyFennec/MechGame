using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public int targetFramerate = 120;
	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFramerate;
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		if (!CursorLockMode.Locked.Equals(Cursor.lockState))
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (Cursor.visible)
		{
			Cursor.visible = false;
		}
	}
}
