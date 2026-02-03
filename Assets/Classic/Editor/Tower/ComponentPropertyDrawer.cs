using Overwave.Classic.Tower;
using UnityEditor;
using UnityEngine;
using Component = Overwave.Classic.Tower.Component;
using Components = Overwave.Classic.Tower.Components;

namespace Overwave.Classic.Editor.Tower
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
                ComponentType.Attack => new Components.Attack(),
                ComponentType.Ammo => new Components.Ammo(),
                ComponentType.SeeHidden => new Components.SeeHidden(),
                ComponentType.SeeFly => new Components.SeeFly(),
                ComponentType.CanDamageLead => new Components.CanDamageLead(),
                ComponentType.RangeModifier => new Components.RangeModifier(),
                _ => null
            };
        }
    }
}