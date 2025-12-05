using UnityEditor;
using UnityEngine;

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
