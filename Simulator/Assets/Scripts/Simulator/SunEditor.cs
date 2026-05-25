#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sun))]
public class SunEditor : Editor {

    Sun sun;
    Editor editor;

	public override void OnInspectorGUI()
	{
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                sun.GenerateSun();
            }
        }

        if (GUILayout.Button("Generate Sun"))
        {
            sun.GenerateSun();
        }

        DrawSettingsEditor(sun.sunSettings, sun.OnSettingsUpdated);
	}

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated)
    {
        if (settings != null)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
        
            {
                Editor editor = CreateEditor(settings);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }
                }
            }
            
        }
    }

	private void OnEnable()
	{
        sun = (Sun)target;
	}
}
#endif