using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InterfaceAttribute))]
public class InterfaceAttriibuteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        InterfaceAttribute interfaceAttribute = attribute as InterfaceAttribute;
    
        Object lastInput = property.objectReferenceValue;

        EditorGUI.ObjectField(position, property, label);

        Object newInput = property.objectReferenceValue;
        if (!newInput || lastInput == newInput)
            return;

        // if it's a GameObject then check all components on it
        if (newInput is GameObject gameObject)
        {
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (!interfaceAttribute.InterfaceType.IsInstanceOfType(component))
                    continue;

                property.objectReferenceValue = component;
                return;
            }
        }
        
        
        // reject the object if it doesn't implement the interface
        if (!interfaceAttribute.InterfaceType.IsInstanceOfType(newInput))
            property.objectReferenceValue = lastInput;
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the height based on the number of fields
        return EditorGUIUtility.singleLineHeight;
    }
}
