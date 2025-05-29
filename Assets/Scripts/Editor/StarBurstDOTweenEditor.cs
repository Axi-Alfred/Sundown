using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StarBurstDOTween))]
public class StarBurstDOTweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StarBurstDOTween script = (StarBurstDOTween)target;

        GUILayout.Space(10);

        if (GUILayout.Button("▶ TEST Star Burst (Inspector)"))
        {
            if (Application.isPlaying)
            {
                script.TriggerBurst();
            }
            else
            {
                Debug.LogWarning("Enter Play Mode to test the star burst.");
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("📋 Copy Code Snippet"))
        {
            string codeSnippet = "FindObjectOfType<StarBurstDOTween>().TriggerBurst();";
            EditorGUIUtility.systemCopyBuffer = codeSnippet;
            Debug.Log("[StarBurstDOTween] Code snippet copied to clipboard:\n" + codeSnippet);
        }
    }
}
