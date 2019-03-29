using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Sample2_3))]
public class Sample2_4 : Editor {

	void OnSceneGUI(){
		Tools.current = Tool.None;
		var component = target as Sample2_3;
		var transform = component.transform;
		transform.position = Handles.PositionHandle (transform.position, transform.rotation);
	}
}
