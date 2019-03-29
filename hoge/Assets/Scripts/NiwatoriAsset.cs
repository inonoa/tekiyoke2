using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (menuName = "Niwatori/Create NiwatoriAsset Instance")]
public class NiwatoriAsset : ScriptableObject {
	[SerializeField]
	string Name;

	[SerializeField]
	List<NiwatoriClass> Objects;

	public static GameObject CreateObject (NiwatoriAsset niwa) {
		var parentObject = new GameObject ("Niwatori");

		foreach (var obj in niwa.Objects) {
			var prefab = NiwatoriClass.GetPrefab (obj.type);

			var childObject = GameObject.Instantiate (prefab, obj.pos, Quaternion.identity);

			var scale = childObject.transform.localScale;
			scale.Scale (obj.scale);
			childObject.transform.localScale = scale;

			childObject.transform.parent = parentObject.transform;
		}

		return parentObject;
	}
}

[System.Serializable]
public class NiwatoriClass {
	public NiwatoriType type;
	public Vector3 pos;
	public Vector3 scale;

	public static GameObject GetPrefab (NiwatoriType type) {
		GameObject prefab;
		switch (type) {
			case NiwatoriType.Niwa:
				prefab = (GameObject) Resources.Load ("Prefabs/niwa");
				break;
			case NiwatoriType.Niwatori:
				prefab = (GameObject) Resources.Load ("Prefabs/niwatori");
				break;
			case NiwatoriType.Wani:
				prefab = (GameObject) Resources.Load ("Prefabs/wani");
				break;
			default:
				prefab = new GameObject ("");
				break;
		}

		return prefab;
	}
}

public enum NiwatoriType {
	Niwatori,
	Niwa,
	Wani
}
