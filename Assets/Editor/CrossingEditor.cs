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
		if (GUILayout.Button("Make Line Connections"))
		{
			MakeLineConnections();
		}

		if (GUILayout.Button("Make Crossing Connections"))
		{
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

	private static void MakeLineConnections()
	{
		Crossing lastCrossing = null;
		foreach (Object obj in Selection.gameObjects)
		{
			if (lastCrossing != null)
			{
				Crossing crossing = (Crossing)obj;
				lastCrossing.points.Add(crossing.transform);
				crossing.points.Add(lastCrossing.transform);
			}
			lastCrossing = (Crossing)obj;
		}
	}

	private void ClearConections()
	{
		foreach (Object obj in targets)
		{
			Crossing crossing = (Crossing)obj;
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
		foreach (Object obj in targets)
		{
			Crossing crossing = (Crossing)obj;
			if(crossing.gameObject.GetComponent<LineRenderer>() == null)
			{
				LineRenderer lineRenderer = crossing.gameObject.AddComponent<LineRenderer>();
				lineRenderer.startWidth = 0.2f;
				lineRenderer.endWidth = 0.2f;
				lineRenderer.material = Resources.Load<Material>("Materials/GreenLine");
				
				List<Transform> pointsToDraw = new List<Transform>();
 				foreach (Transform point in crossing.points)
				{
					foreach(Object blub in targets){
						if (((Crossing) blub).transform == point )
						{
							pointsToDraw.Add(point);
						}
					}
				}
				lineRenderer.positionCount = 2 * pointsToDraw.Count;

				foreach (Transform point in pointsToDraw)
				{
					int index = pointsToDraw.IndexOf(point);
					lineRenderer.SetPosition(2 * index, crossing.transform.position + 0.2f * Vector3.up);
					lineRenderer.SetPosition(2 * index + 1, point.position + 0.2f * Vector3.up);
				}
			}
		}
	}
}
