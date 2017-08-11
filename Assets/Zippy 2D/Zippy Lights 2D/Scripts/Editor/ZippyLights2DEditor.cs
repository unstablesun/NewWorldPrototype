using UnityEngine;
using System.Collections;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(ZippyLights2D))]
public class ZippyLights2DEditor : Editor {
	GUIStyle lab;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		DrawCustomInspector();
	}

	void CheckIdle(ZippyLights2D t) {

		if (!Application.isPlaying) return;
		if (t.idle)
			GUI.color = Color.green;
		else
			GUI.color = Color.gray;
		EditorGUILayout.BeginHorizontal("Box");
		GUILayout.Label("ZippyLight2D is idle: \nUsing less resources.", lab);
		EditorGUILayout.EndHorizontal();
		GUI.color = Color.white;


		if (!t.lightEnabled)
			GUI.color = Color.green;
		else
			GUI.color = Color.gray;
		EditorGUILayout.BeginHorizontal("Box");
		GUILayout.Label("ZippyLight2D not rendered: \nDisabled and using very few resources.", lab);
		EditorGUILayout.EndHorizontal();
		GUI.color = Color.white;
	}


	void CheckResolution(ZippyLights2D t) {
		if (t.resolution <= 360) return;
		GUI.color = Color.red;
		EditorGUILayout.BeginHorizontal("Box");
		GUILayout.Label("ZippyLight2D resolution set high: \nResolution not suitable for mobile.", lab);
		EditorGUILayout.EndHorizontal();
		GUI.color = Color.white;
	}

	void CheckUnityLight(ZippyLights2D t) {
		if (t.unityLight && t.unityLight.renderMode != LightRenderMode.ForceVertex) {
			GUI.color = Color.red;
			EditorGUILayout.BeginHorizontal("Box");
			GUILayout.Label(t.unityLight + "<b>\nRenderMode slow on mobile.</b> \nSet to [Not Important] for mobile.", lab);
			EditorGUILayout.EndHorizontal();
			GUI.color = Color.white;
		}
	}

	public void DrawCustomInspector() {
		ZippyLights2D t = (ZippyLights2D)target;
		if (!Application.isPlaying) {
			if (GUILayout.Button("Update")) {
				t.ForceUpdate();
			}
		}
		lab = new GUIStyle();
		lab.richText = true;
		CheckIdle(t);
		CheckUnityLight(t);
		CheckResolution(t);
	}
}