using System;
using UnityEditor;
using UnityEngine;

public class EditorMenu : EditorWindow
{
    private const float defaultSpace = 10f;

    [MenuItem("Tools/EditorMenu")]
    private static void Open()
    {
        GetWindow<EditorMenu>("EditorMenu");
    }

    private void Button(Action _func, float _space = defaultSpace)
    {
        GUILayout.Space(_space);
        if (GUILayout.Button(_func.Method.Name)) _func();
    }

    private void Test()
    {
        //GUI.backgroundColor = Color.red;
        //GUILayout.Label("Version Tool", EditorStyles.boldLabel);
        //string textValue = EditorGUILayout.TextField("Text", textValue);
    }

    private void OnGUI()
    {
        Button(VersionUpdate);
    }

    private void VersionUpdate()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var newVersion = $"0.{today}.0";

        if (!string.Equals(newVersion, PlayerSettings.bundleVersion))
        {
            PlayerSettings.bundleVersion = newVersion;
            PlayerSettings.Android.bundleVersionCode++;

            Debug.Log($"현재 버전{PlayerSettings.bundleVersion}\n코드 : {PlayerSettings.Android.bundleVersionCode}");
        }
    }
}
