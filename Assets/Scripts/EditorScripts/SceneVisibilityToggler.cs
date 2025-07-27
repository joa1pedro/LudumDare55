using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneVisibilityOverlaySingleSelection
{
    private static Vector2 scrollPos;
    
    private const string TargetSceneName = "MainMenu";
    
    static SceneVisibilityOverlaySingleSelection()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        EditorApplication.hierarchyChanged += () => SceneView.RepaintAll();
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        var activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid() || activeScene.name != TargetSceneName)
            return; // Don't draw outside the target scene
        
        var trackedObjects = GameObject.FindObjectsOfType<VisibilityTrackedObject>(true);
        if (trackedObjects == null || trackedObjects.Length == 0)
            return;

        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(10, 10, 200, 200), "Main Menu Navigator", GUI.skin.window);

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        // Find which is currently active (visible)
        GameObject activeGO = null;
        foreach (var tracked in trackedObjects)
        {
            if (tracked.gameObject.activeSelf)
            {
                activeGO = tracked.gameObject;
                break;
            }
        }

        foreach (var tracked in trackedObjects)
        {
            var go = tracked.gameObject;
            if (go == null) continue;

            bool isActive = go == activeGO;

            EditorGUI.BeginChangeCheck();
            bool newActive = GUILayout.Toggle(isActive, go.name, "Radio");
            if (EditorGUI.EndChangeCheck() && newActive != isActive)
            {
                Undo.RecordObjects(System.Array.ConvertAll(trackedObjects, t => t.gameObject), "Toggle Visibility Single");
                // Disable all others
                foreach (var t in trackedObjects)
                {
                    if (t.gameObject != null)
                        t.gameObject.SetActive(false);
                }
                // Enable the selected one
                go.SetActive(true);
                EditorUtility.SetDirty(go);
            }
        }

        GUILayout.EndScrollView();

        if (GUILayout.Button("Refresh"))
        {
            SceneView.RepaintAll();
        }

        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
