using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class VisibilityTrackedObjectHierarchyLabel
{
    static VisibilityTrackedObjectHierarchyLabel()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        // Get GameObject from instanceID
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null)
            return;

        // Check if it has VisibilityTrackedObject
        if (go.GetComponent<VisibilityTrackedObject>() == null)
            return;

        // Prepare label text based on active state
        string label = go.activeSelf ? "Active" : "Inactive";

        // Calculate label rect (right side of the hierarchy entry)
        var labelStyle = new GUIStyle(EditorStyles.label);
        labelStyle.alignment = TextAnchor.MiddleRight;
        labelStyle.normal.textColor = go.activeSelf ? Color.green : Color.red;
        labelStyle.fontStyle = FontStyle.Bold;

        // Position label a bit to the right
        Rect labelRect = new Rect(selectionRect);
        labelRect.x = labelRect.xMax - 60; // 60 pixels from right edge
        labelRect.width = 60;

        EditorGUI.LabelField(labelRect, label, labelStyle);
    }
}