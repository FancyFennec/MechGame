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
        boostSlider.value = movementController.BoostingThreshold - movementController.BoostingTime;
        boostbadBoarder.color = movementController.IsBoostingOnCooldown ? Color.red : Color.white;
    }
}
