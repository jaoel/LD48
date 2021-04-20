using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : EditorWindow {
    private GameObject prefab = null;
    private Vector2 scrollPosition = Vector2.zero;
    private bool keepName = false;

    [MenuItem("GameObject/Replace with Prefab", false, 0)]
    static void ReplaceMenuItem() {
        // Get existing open window or if none, make a new one:
        ReplaceWithPrefab window = (ReplaceWithPrefab)GetWindow(typeof(ReplaceWithPrefab));
        window.Show();
    }

    private void OnSelectionChange() {
        Repaint();
    }

    private void OnGUI() {
        prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
        keepName = EditorGUILayout.Toggle("Keep name", keepName);
        GameObject[] selections = Selection.gameObjects.Where(x => !PrefabUtility.IsPartOfPrefabAsset(x)).ToArray();
        List<Object> newObjects = new List<Object>();

        EditorGUI.BeginDisabledGroup(selections.Length == 0 || prefab == null);
        if (GUILayout.Button("Replace") && prefab != null) {
            foreach (GameObject selectedObject in selections) {
                GameObject replacedObject = PrefabUtility.InstantiatePrefab(prefab, selectedObject.transform.parent) as GameObject;
                if (replacedObject != null) {
                    replacedObject.transform.localPosition = selectedObject.transform.localPosition;
                    replacedObject.transform.localRotation = selectedObject.transform.localRotation;
                    replacedObject.transform.localScale = selectedObject.transform.localScale;
                    replacedObject.transform.SetSiblingIndex(selectedObject.transform.GetSiblingIndex());
                    if (keepName) {
                        replacedObject.name = selectedObject.name;
                    }
                    Undo.RegisterCreatedObjectUndo(replacedObject, "Replace with Prefab");
                    Undo.DestroyObjectImmediate(selectedObject);
                    newObjects.Add(replacedObject);
                } else {
                    Debug.LogWarning($"Unable to replace {selectedObject.name} with {prefab.name}");
                }

            }
            Selection.objects = newObjects.ToArray();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Label($"{selections.Length} selected objects will be replaced:", EditorStyles.boldLabel);
        EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (GameObject selectedObject in selections) {
            if (selectedObject != null) {
                GUILayout.Label(selectedObject.name);
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
