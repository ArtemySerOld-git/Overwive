using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Overwave.Classic.Editor
{
    public class CreatorWindow : EditorWindow
    {
        private enum Section { Enemy, Tower, GameMode, Unit }
        private Section _section = Section.Enemy;

        private EnemyCreator _enemyCreator;
        
        [MenuItem("Tools/Overwave/Classic/Creator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CreatorWindow>("Creator");
            window.Show();
        }

        private void OnEnable()
        {
            _enemyCreator ??= new EnemyCreator();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    if (GUILayout.Toggle(_section == Section.Enemy, "Enemy", "Button", GUILayout.MinWidth(100)))
                        _section = Section.Enemy;
                }
                EditorGUILayout.EndVertical();

                DrawNeed();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawNeed()
        {
            switch (_section)
            {
                case Section.Enemy: _enemyCreator.Draw(); break;
                default: break;
            }
        }

        private class EnemyCreator
        {
            private enum Section { Standard, Movement, Life, Other }
            private Section _section = Section.Standard;
            
            private string _name;
            
            private readonly Classic.Enemy.Config _config;
            private readonly SerializedObject _target;

            private readonly SerializedProperty _speedProp;
            private readonly SerializedProperty _rotationSpeedProp;
            
            private readonly SerializedProperty _healthProp;
            private readonly SerializedProperty _componentsProp;
            
            private readonly SerializedProperty _animationsCountProp;
            private readonly SerializedProperty _classTypeProp;
            private readonly SerializedProperty _translationsProp;

            public EnemyCreator()
            {
                _config = CreateInstance<Classic.Enemy.Config>();
                _target = new SerializedObject(_config);

                _speedProp             = GetProperty("Speed");
                _rotationSpeedProp     = GetProperty("RotationSpeed");
                
                _healthProp            = GetProperty("Health");
                _componentsProp        = GetProperty("Components");
                
                _animationsCountProp   = GetProperty("AnimationsCount");
                _classTypeProp         = GetProperty("ClassType");
                _translationsProp      = GetProperty("Translations");
            }
            
            public void Draw()
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    {
                        if (GUILayout.Toggle(_section == Section.Standard, "Standard", "Button", GUILayout.MinWidth(100)))
                            _section = Section.Standard;
                        if (GUILayout.Toggle(_section == Section.Movement, "Movement", "Button", GUILayout.MinWidth(100)))
                            _section = Section.Movement;
                        if (GUILayout.Toggle(_section == Section.Life, "Life", "Button", GUILayout.MinWidth(100)))
                            _section = Section.Life;
                        if (GUILayout.Toggle(_section == Section.Other, "Other", "Button", GUILayout.MinWidth(100)))
                            _section = Section.Other;
                    }
                    EditorGUILayout.EndHorizontal();

                    DrawNeed();
                }
                EditorGUILayout.EndVertical();
            }

            private void DrawNeed()
            {
                switch (_section)
                {
                    case Section.Standard: DrawStandard(); break;
                    case Section.Movement: DrawMovement(); break;
                    case Section.Life: DrawLife(); break;
                    case Section.Other: DrawOther(); break;
                }
            }

            private void DrawStandard()
            {
                _name = EditorGUILayout.TextField("Name", _name);
                
                EditorGUILayout.Space();

                if (GUILayout.Button("Build"))
                    Build();
            }

            private void DrawMovement()
            {
                EditorGUILayout.PropertyField(_speedProp, true);
                EditorGUILayout.PropertyField(_rotationSpeedProp, true);
            }

            private void DrawLife()
            {
                EditorGUILayout.PropertyField(_healthProp, true);
                EditorGUILayout.PropertyField(_componentsProp, true);
            }
            
            private void DrawOther()
            {
                EditorGUILayout.PropertyField(_animationsCountProp, true);
                EditorGUILayout.PropertyField(_classTypeProp, true);
                EditorGUILayout.PropertyField(_translationsProp, true);
            }

            private void Build()
            {
                var path = Path.Combine(Application.dataPath, "Classic", "Content", "Enemy", _name);
                
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                Directory.CreateDirectory(Path.Combine(path, "Animations"));
                Directory.CreateDirectory(Path.Combine(path, "Materials"));
                Directory.CreateDirectory(Path.Combine(path, "Textures"));
                Directory.CreateDirectory(Path.Combine(path, "Prefabs"));
                Directory.CreateDirectory(Path.Combine(path, "Models"));

                _name = _name.Replace(" ", "_");

                _target.ApplyModifiedProperties();
                _config.Id = $"classic.enemy.{_name.ToLower()}";
                
                var relativePath = Path.Combine("Assets", "Classic", "Content", "Enemy", _name);
                AssetDatabase.CreateAsset(_config, Path.Combine(relativePath, $"Enemy_{_name}_Config.asset"));
                
                var controller = AnimatorController.CreateAnimatorControllerAtPath(Path.Combine(relativePath,
                    "Animations", $"Enemy_{_name}_AnimatorController.controller"));
                
                controller.AddParameter("Variant", AnimatorControllerParameterType.Float);
                controller.AddParameter("SpeedMultiplier", AnimatorControllerParameterType.Float);
                
                controller.CreateBlendTreeInController("Variants", out var tree);
                if (_config.AnimationsCount == 1)
                    tree.AddChild(null);
                else
                {
                    for (var i = 0; i < _config.AnimationsCount; i++)
                        tree.AddChild(null, i);
                }
            }
            
            private SerializedProperty GetProperty(string name)
                => _target.FindProperty($"<{name}>k__BackingField");
        }
    }
}