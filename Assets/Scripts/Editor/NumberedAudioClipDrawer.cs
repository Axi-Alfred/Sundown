using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NumberedAudioClip))]
public class NumberedAudioClipDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Dynamic height based on expanded clips array
        SerializedProperty clipsProp = property.FindPropertyRelative("clips");
        int clipLines = clipsProp.isExpanded ? Mathf.Max(clipsProp.arraySize, 1) + 2 : 1;
        return EditorGUIUtility.singleLineHeight * (clipLines + 4) + 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty clipsProp = property.FindPropertyRelative("clips");
        SerializedProperty descProp = property.FindPropertyRelative("description");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 4f;
        float y = position.y;

        // --- 1. Clips list
        Rect clipsRect = new Rect(position.x, y, position.width, lineHeight);
        EditorGUI.PropertyField(clipsRect, clipsProp, new GUIContent("Clips"), true);
        y += EditorGUI.GetPropertyHeight(clipsProp, true) + spacing;

        // --- 2. Code usage
        int index = property.propertyPath.Contains("Array.data[")
            ? int.Parse(property.propertyPath.Split('[', ']')[1]) + 1
            : -1;

        string usage = $"sfxLibrary.Play({index});";
        Rect usageRect = new Rect(position.x, y, position.width - 120f, lineHeight);
        EditorGUI.LabelField(usageRect, "Code to use:", usage);

        // --- 3. Copy button
        Rect buttonRect = new Rect(position.x + position.width - 110f, y, 100f, lineHeight);
        if (GUI.Button(buttonRect, "Copy Code"))
        {
            EditorGUIUtility.systemCopyBuffer = usage;
            Debug.Log($"Copied to clipboard: {usage}");
        }

        y += lineHeight + spacing;

        // --- 4. Description box
        Rect descRect = new Rect(position.x, y, position.width, lineHeight * 2);
        EditorGUI.PropertyField(descRect, descProp, new GUIContent("Description"));
    }
}
