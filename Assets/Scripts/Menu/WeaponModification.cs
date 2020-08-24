using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponModification : MonoBehaviour
{
    GameObject player;
    GameObject[] gameObjects;
    void Start()
    {

        gameObjects = SceneManager.GetSceneByName("MainScene").GetRootGameObjects();
        player = gameObjects.ToList().Find(gameObject => gameObject.name == "Player");
        player.GetComponent<PlayerWeaponController>().ChangePrimaryWeapon(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
