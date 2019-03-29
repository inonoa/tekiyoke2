using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Sample1_7 : EditorWindow {

	[MenuItem ("CustomMenu/Create EditorGUI Example")]
	static void CreateWindow () {
		GetWindow<Sample1_7> ();
	}

	static bool flag1 = false;
	static bool flag2 = false;
	static int num1 = 1;
	static int num2 = 2;
	static int num3 = 3;

	static bool on = false;
	static int selected = 0;

	void OnGUI () {
		EditorGUI.BeginChangeCheck ();

		flag1 = EditorGUILayout.Toggle (flag1);
		flag2 = EditorGUILayout.Toggle ("Label", flag2);

		if (EditorGUI.EndChangeCheck ()) {
			if (flag1 ^ flag2) {
				Debug.Log ("こういう風に出せるというわけ");
			}
		}

		num1 = EditorGUILayout.IntField (num1);
		var vec = EditorGUILayout.Vector2IntField ("hoge", new Vector2Int (num2, num3));
		num2 = vec.x;
		num3 = vec.y;

		using (new EditorGUILayout.HorizontalScope ()) {
			GUILayout.Button ("a");
			GUILayout.Button ("b");
		}

		on = GUILayout.Toggle (on, on ? "on" : "off", "button");
		selected = GUILayout.Toolbar (selected, new string[] { "1", "2", "3" });

		using (new Sample1_8 ()) {
			GUILayout.Button ("a");
			using (new Sample1_8 ()) {
				GUILayout.Button ("b");
			}
		}
	}
}
