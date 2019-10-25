using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorNote : MonoBehaviour
{
	[TextArea]
	public string text;
}

[CustomEditor(typeof(EditorNote))]
public class EditorNoteEditor : Editor
{
	bool init;
	EditorNote _target;
	GUILayoutOption options;
	Color defaultColor = Color.blue;

	/*private void OnEnable()
	{
		if (true) {
			UnityEditorInternal.ComponentUtility.MoveComponentUp((EditorNote)target);
		}
	}*/

	public override void OnInspectorGUI()
	{
		if (!init) {
			init = true;
			_target = (EditorNote)target;
			SetTexture(200, 200, Color.red);
		}
		DrawLine();
		GUILayout.BeginVertical();
		_target.text = GUILayout.TextArea(_target.text, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(72f));
		GUILayout.EndVertical();
		DrawLine();


		GUILayout.BeginHorizontal(t, null, GUILayout.ExpandHeight(true));
		_target.text = GUILayout.TextArea(_target.text, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(72f));
		GUILayout.EndHorizontal();

	}

	private void DrawLine()
	{
		DrawUILine(Color.cyan, 2, 10);
	}

	Texture2D t;

	public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
	{
		Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		r.width += 6;
		EditorGUI.DrawRect(r, color);
	}

	private void DrawLine(Color lineColor, float height)
	{
		GUILayoutOption[] options = {
			GUILayout.ExpandWidth(true),
			GUILayout.Height(height)
		};

		EditorGUILayout.BeginHorizontal();
		GUILayout.Box(t, options);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(t, options);
		EditorGUILayout.EndHorizontal();
	}

	void SetTexture(int width, int height, Color c)
	{
		t = new Texture2D(width, height);
		Color[] color = new Color[t.width * t.height];
		for(int i = 0; i < t.width * t.height; i++) {
			color[i] = c;
		}
		t.SetPixels(0, 0, t.width, t.height, color);
	}
}
