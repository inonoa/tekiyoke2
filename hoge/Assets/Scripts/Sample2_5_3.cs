using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomPropertyDrawer (typeof (Sample2_5_2))]
public class Sample2_5_3 : PropertyDrawer {
	public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
		var target = (Sample2_5_2) attribute;
		if (property.propertyType == SerializedPropertyType.Integer) {
			EditorGUI.LabelField (rect,"整数");
		} else {
			EditorGUI.LabelField (rect, label);
		}
	}
}
