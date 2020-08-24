using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

	bool isMenuOpen = false;
    void Start()
    {
		SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isMenuOpen)
			{
				SceneManager.GetSceneByName("MainScene").GetRootGameObjects().ToList().ForEach(
					obj => obj.SetActive(true));
				SceneManager.UnloadSceneAsync("MenuScene");
			} else
			{
				SceneManager.GetSceneByName("MainScene").GetRootGameObjects().ToList().ForEach(
					obj => obj.SetActive(false));
				SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
			}
			isMenuOpen = !isMenuOpen;
		}
    }
}
