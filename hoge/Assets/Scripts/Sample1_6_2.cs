using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor (typeof (Sample1_6_1))]
public class Sample1_6_2 : Editor {

	Sample1_6_1 component;

	void OnEnable () {
		component = (Sample1_6_1) target;
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();

		EditorGUILayout.LabelField ($"戦闘力:{component.atk*2+component.def}");
	}
}
