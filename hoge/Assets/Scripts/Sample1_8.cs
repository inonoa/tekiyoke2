using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Sample1_8 : GUI.Scope {
	Color prevcolor;
	public Sample1_8 () {
		prevcolor = GUI.backgroundColor;
		var color = Color.HSVToRGB (Random.Range (0f, 1f), 1, 1);
		GUI.backgroundColor = color;
	}

	protected override void CloseScope () {
		GUI.backgroundColor = prevcolor;
	}
}
