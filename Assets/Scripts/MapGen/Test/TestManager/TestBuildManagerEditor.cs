using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(TestBuildManager))]
public class TestBuildManagerEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestBuildManager myScript = (TestBuildManager)target;

        if (GUILayout.Button("Generate new Map")) {
            myScript.GenerateMap();
        }
    }
}

#endif