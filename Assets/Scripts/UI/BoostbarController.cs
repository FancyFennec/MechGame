using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostbarController : MonoBehaviour
{
    private Slider boostSlider;
    [SerializeField]
    private Image boostbadBoarder;
    [SerializeField]
    private PlayerMovementController movementController;
    void Start()
    {
        boostSlider = GetComponent<Slider>();
    }

    void Update()
    {
        boostSlider.value = movementController.boostingThreshold - movementController.boostingTime;
        boostbadBoarder.color = movementController.isBoostingOnCooldown ? Color.red : Color.white;
    }
}
