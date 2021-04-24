using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WrapInEmpty : EditorWindow {
    [MenuItem("GameObject/Wrap in Empty", false, 0)]
    public static void Wrap(MenuCommand menuCommand) {
        //Prevent executing multiple times when right-clicking.
        if (Selection.objects.Length > 1) {
            if (menuCommand.context != Selection.objects[0]) {
                return;
            }
        }

        foreach (var obj in Selection.objects) {
            if (obj is GameObject gameObject && gameObject.scene.IsValid()) {
                int siblingIndex = gameObject.transform.GetSiblingIndex();
                string undoStr = $"Wrap in Empty {siblingIndex}";

                GameObject newGameObject = new GameObject(gameObject.name);
                newGameObject.transform.position = gameObject.transform.position;
                newGameObject.transform.rotation = gameObject.transform.rotation;
                Undo.RegisterCreatedObjectUndo(newGameObject, undoStr);

                Undo.SetTransformParent(gameObject.transform, newGameObject.transform, true, undoStr);
                newGameObject.transform.SetSiblingIndex(siblingIndex);
            }
        }
    }
}
