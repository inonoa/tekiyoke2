using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Sample2_3 : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Handles.color = Color.red;
		Handles.DrawWireDisc (new Vector3 (0, 1, 0), Vector3.forward, 1);
		Gizmos.DrawLine (new Vector3 (0, 0, 0), new Vector3 (-2, -1, 0));
		Gizmos.DrawLine (new Vector3 (0, 0, 0), new Vector3 (2, -1, 0));
		Gizmos.DrawLine (new Vector3 (0, 0, 0), new Vector3 (0, -2, 0));
		Gizmos.DrawLine (new Vector3 (0, -2, 0), new Vector3 (1, -4, 0));
		Gizmos.DrawLine (new Vector3 (0, -2, 0), new Vector3 (-1, -4, 0));
	}
}
