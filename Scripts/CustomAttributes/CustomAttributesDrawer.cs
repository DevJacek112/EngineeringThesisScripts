#if (UNITY_EDITOR) 

using UnityEditor;
using UnityEngine;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(NotNullAttribute))]
    public class CustomAttributesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.HelpBox(position, $"{property.displayName} cannot be null", MessageType.Error);
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            return EditorGUIUtility.singleLineHeight;
        }
    }
    
    [CustomPropertyDrawer(typeof(GreaterThanZeroAttribute))]
    public class GreaterThanZeroDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            
            Rect labelPosition = new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight);
            Rect fieldPosition = new Rect(position.x + labelWidth + 1, position.y, position.width - labelWidth, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.LabelField(labelPosition, label);
            EditorGUI.PropertyField(fieldPosition, property, GUIContent.none);
            
            Rect errorPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
            
            if (property.propertyType == SerializedPropertyType.Float && property.floatValue <= 0)
            {
                EditorGUI.HelpBox(errorPosition, $"{property.displayName} must be greater than zero", MessageType.Error);
            }
            else if (property.propertyType == SerializedPropertyType.Integer && property.intValue <= 0)
            {
                EditorGUI.HelpBox(errorPosition, $"{property.displayName} must be greater than zero", MessageType.Error);
            }
            else if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.HelpBox(errorPosition, "Use GreaterThanZero with float or int.", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if ((property.propertyType == SerializedPropertyType.Float && property.floatValue <= 0) ||
                (property.propertyType == SerializedPropertyType.Integer && property.intValue <= 0) ||
                (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer))
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }

            return EditorGUIUtility.singleLineHeight;
        }
    }
}

#endif