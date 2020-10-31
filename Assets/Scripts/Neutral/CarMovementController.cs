using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    public GameObject target;
    public GameObject path;
    public bool clockwise;

    private Rigidbody rb;
    private List<Transform> points = new List<Transform>();
    private int targetIndex = 0;
    private int nextIndex = 0;
    private int lastIndex = 0;

    private Vector3 targetDirection = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private float distance = 0f;



    void Start()
    {
        foreach(Transform child in path.transform)
		{
            points.Add(child);
        }
        
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (distance < 0.6)
		{
            updateIndeces();
            targetPosition = getTargetPosition();
            target.transform.position = targetPosition;
            Debug.Log(targetIndex);
		}
        targetDirection = (targetPosition - transform.position);
        distance = targetDirection.magnitude;
        targetDirection = (transform.forward + 0.03f * targetDirection.normalized).normalized;
        if (Vector3.Dot(transform.up, Vector3.up) > 0.8f)
		{
            rb.MovePosition(transform.position + targetDirection * Time.fixedDeltaTime * 2f);
        }
        rb.MoveRotation(Quaternion.LookRotation((transform.forward + targetDirection).normalized));
    }

    private Vector3 getTargetPosition()
	{
        Vector3 backwards = getBackwardDirection().normalized;
        Vector3 forwards = getForwardDirection().normalized;

        Vector3 crossProd = Vector3.Cross(backwards, forwards);
		if (crossProd.y > 0) {
            return points[targetIndex].position - 2 * (backwards + forwards).normalized;
        } else if(crossProd.y < 0) {
            return points[targetIndex].position + 2 * (backwards + forwards).normalized;
        } else {
            return points[targetIndex].position - 2 * Vector3.Cross(forwards, Vector3.up).normalized;
        }
    }

    private Vector3 getBackwardDirection()
    {
        return points[lastIndex].position - points[targetIndex].position;
    }

    private Vector3 getForwardDirection()
    {
        return points[nextIndex].position - points[targetIndex].position;
    }

    private void updateIndeces()
	{
        lastIndex = targetIndex;
        targetIndex = nextIndex;
        Crossing crossing = points[targetIndex].GetComponent<Crossing>();
		if (crossing != null)
		{
			Transform nextPoint = crossing.points[Random.Range(0, crossing.points.Count)];
			while (nextPoint == points[lastIndex])
			{
                nextPoint = crossing.points[Random.Range(0, crossing.points.Count)];
            }
            nextIndex = points.IndexOf(nextPoint);
        } else
		{

            nextIndex = (targetIndex + points.Count + (clockwise ? 1 : -1)) % points.Count;
        }
	}
}
