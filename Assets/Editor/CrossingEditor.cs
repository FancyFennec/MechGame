using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Crossing))]
[CanEditMultipleObjects]
public class CrossingEditor : Editor
{
 	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI();

		if (GUILayout.Button("Add Crossing Connections"))
		{
			ClearLines();
			MakeCrossingConnections();
			AddLines();
		}

		if (GUILayout.Button("Clear Connections"))
		{
			ClearConections();
			ClearLines();
		}

		if (GUILayout.Button("Add Lines"))
		{
			AddLines();
		}

		if (GUILayout.Button("Clear Lines"))
		{
			ClearLines();
		}
	}

	private void MakeCrossingConnections()
	{
		List<Crossing> crossings = new List<Crossing>();
		Crossing activeCrossing = ((GameObject)Selection.activeObject).GetComponent<Crossing>();
		foreach (Object obj in targets)
		{
			Crossing crossing = (Crossing)obj;
			if (crossing != null && crossing != activeCrossing)
			{
				crossings.Add(crossing);
			}
		}
		crossings.ForEach(cr =>
		{
			if (!cr.points.Contains(activeCrossing.transform))
			{
				cr.points.Add(activeCrossing.transform);
			}
			if (!activeCrossing.points.Contains(cr.transform))
			{
				activeCrossing.points.Add(cr.transform);
			}
		});
	}

	private void ClearConections()
	{
		foreach (Object obj in targets)
		{
			Crossing crossing = (Crossing)obj;
			foreach(Transform point in crossing.points)
			{
				Crossing neighbourCrossing = point.GetComponent<Crossing>();
				neighbourCrossing.points.Remove(crossing.transform);
			}
			crossing.points.Clear();
		}
	}

	private void ClearLines()
	{
		foreach (Crossing crossing in FindObjectsOfType<Crossing>())
		{
			LineRenderer lineRenderer = crossing.gameObject.GetComponent<LineRenderer>();
			if (lineRenderer != null)
			{
				DestroyImmediate(lineRenderer);
			}
		}
	}

	private void AddLines()
	{
		GameObject targetGO = (GameObject)Selection.activeObject;
		Crossing[] allCrossings = targetGO.transform.parent.GetComponentsInChildren<Crossing>();

		foreach(Crossing crossing in allCrossings)
		{
			LineRenderer lineRenderer = getLineRenderer(crossing);
			List<Transform> pointsToDraw = getPointsToDraw(allCrossings, crossing);
			lineRenderer.positionCount = 2 * pointsToDraw.Count;

			foreach (Transform point in pointsToDraw)
			{
				int index = pointsToDraw.IndexOf(point);
				lineRenderer.SetPosition(2 * index, crossing.transform.position + 0.2f * Vector3.up);
				lineRenderer.SetPosition(2 * index + 1, point.position + 0.2f * Vector3.up);
			}
		}
	}

	private static List<Transform> getPointsToDraw(Crossing[] allCrossings, Crossing cross1)
	{
		List<Transform> pointsToDraw = new List<Transform>();
		foreach (Crossing cross2 in allCrossings)
		{
			if (cross2.points.Contains(cross1.transform))
			{
				pointsToDraw.Add(cross2.transform);
			}
		}

		return pointsToDraw;
	}

	private static LineRenderer getLineRenderer(Crossing crossing)
	{
		LineRenderer lineRenderer;
		if (crossing.gameObject.GetComponent<LineRenderer>() == null)
		{
			lineRenderer = crossing.gameObject.AddComponent<LineRenderer>();
			lineRenderer.startWidth = 0.2f;
			lineRenderer.endWidth = 0.2f;
			lineRenderer.material = Resources.Load<Material>("Materials/GreenLine");
		}
		else
		{
			lineRenderer = crossing.gameObject.GetComponent<LineRenderer>();
		}

		return lineRenderer;
	}
}
