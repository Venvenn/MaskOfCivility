using Escalon.Nova;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationConfiguration))]
public class AnimationConfigurationEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty targetField = property.FindPropertyRelative("TargetType");
        AnimationEaseType animationEaseType = (AnimationEaseType)targetField.enumValueIndex;
        
        Rect drawRect = new Rect(position.x,position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(drawRect, property.isExpanded, animationEaseType.ToString()); 
        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        
        if (property.isExpanded)
        {
            SerializedProperty chainField = property.FindPropertyRelative("ChainType");
            SerializedProperty startField = property.FindPropertyRelative("Start");
            SerializedProperty endField = property.FindPropertyRelative("End");
            SerializedProperty startColourField = property.FindPropertyRelative("StartColour");
            SerializedProperty endColourField = property.FindPropertyRelative("EndColour");
            SerializedProperty localField = property.FindPropertyRelative("Local");
            SerializedProperty durationField = property.FindPropertyRelative("Duration");
            SerializedProperty extentField = property.FindPropertyRelative("Extent");
            SerializedProperty shapeField = property.FindPropertyRelative("Shape");

            EditorGUI.PropertyField(drawRect, targetField);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, chainField);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            switch (animationEaseType)
            {
                case AnimationEaseType.Position:
                    EditorGUI.PropertyField(drawRect, startField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, endField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, localField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    break;
                case AnimationEaseType.Scale:
                    EditorGUI.PropertyField(drawRect, startField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, endField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    break;
                case AnimationEaseType.Rotation:
                    EditorGUI.PropertyField(drawRect, startField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, endField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, localField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    break;
                case AnimationEaseType.Colour:
                    EditorGUI.PropertyField(drawRect, startColourField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, endColourField);
                    drawRect.y += EditorGUIUtility.singleLineHeight+ EditorGUIUtility.standardVerticalSpacing;
                    break;
            }
            
            EditorGUI.PropertyField(drawRect, durationField);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; 
            EditorGUI.PropertyField(drawRect, extentField);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, shapeField);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        EditorGUI.EndFoldoutHeaderGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            SerializedProperty targetField = property.FindPropertyRelative("TargetType");
            AnimationEaseType animationEaseType = (AnimationEaseType)targetField.enumValueIndex;
            float specificValueCountSubtraction = animationEaseType switch
            {
                AnimationEaseType.Colour or AnimationEaseType.Scale => 3,
                AnimationEaseType.None => 5,
                _ => 2
            };
            specificValueCountSubtraction *= EditorGUIUtility.singleLineHeight;
            float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) - specificValueCountSubtraction + EditorGUIUtility.standardVerticalSpacing;
            return totalHeight;
        }
        else
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
