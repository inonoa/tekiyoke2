using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class sample1_3 {

	[MenuItem ("CustomMenu/これを押すと何が起こる？ _@")]
	public static void KusoFunc () {
		var path = "CustomMenu/これを押すと何が起こる？ _@";
		var @checked = Menu.GetChecked (path);
		Menu.SetChecked (path, !@checked);
	}

	[MenuItem ("CustomMenu/a", true)]
	public static void InvalidFunc () {

	}

	[MenuItem ("PriorityMenu/a", false, 1)]
	public static void Func1 () {

	}

	[MenuItem ("PriorityMenu/b", false, 2)]
	public static void Func2 () {

	}

	[MenuItem ("PriorityMenu/b", true)]
	public static bool ValidateFunc2 () {
		return false;
	}

	[MenuItem ("PriorityMenu/c", false, 13)]
	public static void Func3 () {

	}

	//CONTEXT/Component/ホゲホゲ で全てのComponent
	[MenuItem ("CONTEXT/Transform/超すごい選択肢")]
	public static void ContextFunc (MenuCommand menuCommand) {
		var transform = (Transform) menuCommand.context;
		transform.position = Vector3.zero;
	}
}
