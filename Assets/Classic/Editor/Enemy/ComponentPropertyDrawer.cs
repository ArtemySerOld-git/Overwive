using UnityEditor;
using UnityEngine;
using Component = Overwave.Classic.Enemy.Component;
using Components = Overwave.Classic.Enemy.Components;
using ComponentType = Overwave.Classic.Enemy.ComponentType;

namespace Overwave.Classic.Editor.Enemy
{
    [CustomPropertyDrawer(typeof(Component))]
    public class ComponentPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var current = property.managedReferenceValue as Component;
            
            var currentType = current?.Type ?? ComponentType.None;
            
            Rect popupRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var selectedType = (ComponentType)EditorGUI.EnumPopup(popupRect, "Type", currentType);

            if (selectedType != currentType)
            {
                property.managedReferenceValue = CreateComponent(selectedType);
                property.serializedObject.ApplyModifiedProperties();
                EditorGUI.EndProperty();
                return;
            }

            if (property.managedReferenceValue != null)
            {
                Rect fieldRect = new(
                    position.x,
                    position.y + EditorGUIUtility.singleLineHeight + 2,
                    position.width,
                    position.height
                );
                
                EditorGUI.PropertyField(fieldRect, property, true);
            }
            
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.managedReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;
            return EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.singleLineHeight + 2;
        }

        private static Component CreateComponent(ComponentType type)
        {
            return type switch
            {
                ComponentType.Hidden => new Components.Hidden(),
                ComponentType.Fly => new Components.Fly(),
                _ => null
            };
        }
    }
}