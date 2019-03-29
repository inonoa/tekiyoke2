using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent (typeof (Collider2D))]
[DisallowMultipleComponent]
[AddComponentMenu ("My Menu/My script")]
[ExecuteInEditMode]
public class sample1_2 : MonoBehaviour {

	[Range (1, 10)]
	[ContextMenuItem ("最高の選択肢", "SuperFunc")]
	public int num1;

	[Range (1, 10)]
	public float num2;

	[Multiline (5)]
	public string multiline;

	[TextArea (2, 3)]
	public string textarea;

	[ColorUsage (true)]
	public Color color;

	[Header ("これはヘッダー")]
	[Space (40)]
	[Tooltip ("これはTooltip")]
	public int hoge;

	[HideInInspector]
	public int fuga;

	[SerializeField]
	private int piyo;

	[ContextMenu ("押すとどうなると思う？")]
	void SuperFunc () {
		num1 = 1;
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Start");
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("Update");
	}
}
