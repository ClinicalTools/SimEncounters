using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResizeCanvas))]
public class ResizeCanvasEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		//seraializedObject.Update();

		//ResizeCanvas _target = (ResizeCanvas)target;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Scale Value");
		ResizeCanvas.resizeValue = EditorGUILayout.Slider(ResizeCanvas.resizeValue, 0f, 1f);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Resize")) {
			//Invert the float range so that it's a zoom-in representation
			ResizeCanvas.ScaleResize(1 - ResizeCanvas.resizeValue);
		}
		//serializedObject.ApplyModifiedProperties();
	}
}
