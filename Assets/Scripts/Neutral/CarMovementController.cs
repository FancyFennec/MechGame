using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementController : MonoBehaviour
{

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(Vector3.Dot(transform.up, Vector3.up) > 0.8f)
		{
            rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * 1f);
        }
    }
}
