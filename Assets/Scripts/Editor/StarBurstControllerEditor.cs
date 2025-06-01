using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StarBurstController))]
public class StarBurstControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector fields
        DrawDefaultInspector();

        // Get the StarBurstController reference
        StarBurstController controller = (StarBurstController)target;

        // Add a big test button
        if (GUILayout.Button("▶ Trigger Star Burst (Inspector Test Button)"))
        {
            if (Application.isPlaying)
            {
                controller.TriggerBurst();
            }
            else
            {
                Debug.LogWarning("Play mode is required to test Star Burst.");
            }
        }
    }
}
