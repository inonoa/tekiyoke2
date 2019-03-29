using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (NiwatoriAsset), true)]
public class NiwatoriAssetInspector : Editor {
	PreviewRenderUtility previewRenderUtility;
	GameObject previewObject;

	void OnEnable () {
		previewRenderUtility = new PreviewRenderUtility (true);

		previewRenderUtility.cameraFieldOfView = 30f;

		previewRenderUtility.camera.farClipPlane = 1000;
		previewRenderUtility.camera.nearClipPlane = 0.3f;

		var component = (NiwatoriAsset) target;
		previewObject = NiwatoriAsset.CreateObject (component);

		previewRenderUtility.AddSingleGO (previewObject);

		var frame = (GameObject) Resources.Load ("Prefabs/GraphicFrame");
		var frameObj = Instantiate (frame);
		previewRenderUtility.AddSingleGO (frameObj);
	}

	//これ動いてなさそう
	void OnValidate () {
		Debug.Log ("ほげ");
		OnDisable ();
		OnEnable ();
	}

	void OnDisable () {
		previewRenderUtility.Cleanup ();
		previewRenderUtility = null;
		previewObject = null;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		DrawPreview (GUILayoutUtility.GetRect (300, 300));
	}

	public override bool HasPreviewGUI () {
		return true;
	}

	public override void OnPreviewGUI (Rect r, GUIStyle background) {
		previewRenderUtility.BeginPreview (r, background);

		var previewCamera = previewRenderUtility.camera;

		previewCamera.transform.position =
			previewObject.transform.position + new Vector3 (0, 0, -10);

		previewCamera.Render ();

		previewRenderUtility.EndAndDrawPreview (r);
	}
}
