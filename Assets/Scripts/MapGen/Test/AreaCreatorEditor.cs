using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(AreaCreator))]
public class AreaCreatorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        AreaCreator myScript = (AreaCreator)target;

        if (GUILayout.Button("Update Object")) {
            myScript.UpdateVisuals();
        }
    }
}
#endif