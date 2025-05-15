using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NumberedAudioClip))]
public class NumberedAudioClipDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty clipsProp = property.FindPropertyRelative("clips");
        int clipLines = clipsProp.isExpanded ? Mathf.Max(clipsProp.arraySize, 1) + 2 : 1;
        return EditorGUIUtility.singleLineHeight * (clipLines + 6) + 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty clipsProp = property.FindPropertyRelative("clips");
        SerializedProperty descProp = property.FindPropertyRelative("description");
        SerializedProperty pitchMin = property.FindPropertyRelative("pitchMin");
        SerializedProperty pitchMax = property.FindPropertyRelative("pitchMax");
        SerializedProperty volMin = property.FindPropertyRelative("volumeMin");
        SerializedProperty volMax = property.FindPropertyRelative("volumeMax");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 4f;
        float y = position.y;

        // --- 1. Clips
        Rect clipsRect = new Rect(position.x, y, position.width, lineHeight);
        EditorGUI.PropertyField(clipsRect, clipsProp, new GUIContent("Clips"), true);
        y += EditorGUI.GetPropertyHeight(clipsProp, true) + spacing;

        // --- 2. Usage
        int index = property.propertyPath.Contains("Array.data[")
            ? int.Parse(property.propertyPath.Split('[', ']')[1]) + 1
            : -1;
        string usage = $"SFX.Play({index});";

        Rect usageRect = new Rect(position.x, y, position.width - 120f, lineHeight);
        EditorGUI.LabelField(usageRect, "Code to use:", usage);

        // --- 3. Copy Button
        Rect buttonRect = new Rect(position.x + position.width - 110f, y, 100f, lineHeight);
        if (GUI.Button(buttonRect, "Copy Code"))
        {
            EditorGUIUtility.systemCopyBuffer = usage;
            Debug.Log($"Copied to clipboard: {usage}");
        }
        y += lineHeight + spacing;

        // --- 4. Description
        Rect descRect = new Rect(position.x, y, position.width, lineHeight * 2);
        EditorGUI.PropertyField(descRect, descProp, new GUIContent("Description"));
        y += lineHeight * 2 + spacing;

        // --- 5. Pitch
        Rect pitchMinRect = new Rect(position.x, y, position.width / 2 - 5, lineHeight);
        Rect pitchMaxRect = new Rect(position.x + position.width / 2 + 5, y, position.width / 2 - 5, lineHeight);
        EditorGUI.PropertyField(pitchMinRect, pitchMin, new GUIContent("Pitch Min"));
        EditorGUI.PropertyField(pitchMaxRect, pitchMax, new GUIContent("Pitch Max"));
        y += lineHeight + spacing;

        // --- 6. Volume
        Rect volMinRect = new Rect(position.x, y, position.width / 2 - 5, lineHeight);
        Rect volMaxRect = new Rect(position.x + position.width / 2 + 5, y, position.width / 2 - 5, lineHeight);
        EditorGUI.PropertyField(volMinRect, volMin, new GUIContent("Volume Min"));
        EditorGUI.PropertyField(volMaxRect, volMax, new GUIContent("Volume Max"));
    }
}
