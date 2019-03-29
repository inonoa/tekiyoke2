using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Sample1_4 : EditorWindow, IHasCustomMenu {

	[MenuItem ("CustomMenu/Create Window")]
	public static void CreateWindow () {
		var window = CreateInstance<Sample1_4> ();
		window.Show ();
	}

	[MenuItem ("CustomMenu/Create Single Window")]
	public static void CreateSingleWindow () {
		GetWindow<Sample1_4> ();
		//GetWindow<Sample1_4> (typeof(SceneView));
	}

	public void AddItemsToMenu (GenericMenu menu) {
		menu.AddItem (new GUIContent ("選択肢"), false, () => {
			Debug.Log ("あ〜あ");
		});
	}
}
