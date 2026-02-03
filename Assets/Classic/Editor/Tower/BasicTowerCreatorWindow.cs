using System.IO;
using Overwave.Classic.Tower;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Overwave.Classic.Editor.Tower
{
    public class BasicTowerCreatorWindow : EditorWindow
    {
        private string _towerName;
        private ushort _levelsCount = 5;
        private ushort _placementCount = 40;
        private LayerMask _allowedPlatforms;
        private ClassType _classType = ClassType.None;
        
        [MenuItem("Tools/Overwave/Classic/Create Tower")]
        public static void ShowWindow()
        {
            var window = GetWindow<BasicTowerCreatorWindow>();
            window.Show();
        }
        
        private void OnGUI()
        {
            _towerName = EditorGUILayout.TextField("Tower Name", _towerName);
            _levelsCount = (ushort)EditorGUILayout.IntSlider("Levels Count", _levelsCount, 0, 10);
            _placementCount = (ushort)EditorGUILayout.IntSlider("Max Placement Count", _placementCount, 0, 40);
            _allowedPlatforms = EditorGUILayout.LayerField("Allowed Platforms", _allowedPlatforms);
            _classType = (ClassType)EditorGUILayout.EnumFlagsField("Class Type", _classType);
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Build"))
            {
                Create(_towerName, _levelsCount, _placementCount, _classType);
                Close();
            }
        }

        public static void Create(string towerName) => Create(towerName, 1);

        public static void Create(string towerName, int levelsCount, int placementCount = 40, ClassType classType = ClassType.None)
        {
            var path = Path.Combine("Assets", "Classic", "Content", "Tower", towerName);
            
            Directory.CreateDirectory(path);
            
            Directory.CreateDirectory(Path.Combine(path, "Animations"));
            Directory.CreateDirectory(Path.Combine(path, "Materials"));
            Directory.CreateDirectory(Path.Combine(path, "Textures"));
            Directory.CreateDirectory(Path.Combine(path, "Prefabs"));
            Directory.CreateDirectory(Path.Combine(path, "Configs"));
            Directory.CreateDirectory(Path.Combine(path, "Models"));

            towerName = towerName.Replace(" ", "_");
            
            var config = CreateInstance<Config>();
            config.Id = "classic.tower." + towerName.ToLower();
            config.MaxPlacementCount = placementCount;
            config.ClassType = classType;

            LevelConfig lastLevel = null;
            for (var i = 0; i < levelsCount; i++)
            {
                var level = CreateInstance<LevelConfig>();
                level.Id = $"{config.Id}.level{i}";
                if (lastLevel) lastLevel.Next = level;
                AssetDatabase.CreateAsset(level, Path.Combine(path, "Configs", $"Tower_{towerName}_Level{i}_Config.asset"));
                
                config.Levels.Add(level);

                lastLevel = level;
            }
            
            var previewConfig = CreateInstance<PreviewConfig>();
            previewConfig.Id = $"{config.Id}.preview";
            AssetDatabase.CreateAsset(previewConfig, Path.Combine(path, "Configs", $"Tower_{towerName}_PreviewConfig.asset"));

            config.Preview = previewConfig;
            
            AssetDatabase.CreateAsset(config, Path.Combine(path, "Configs", $"Tower_{towerName}_Config.asset"));

            var controller = AnimatorController.CreateAnimatorControllerAtPath(Path.Combine(path, "Animations",
                $"Tower_{towerName}_AnimatorController.controller"));
            controller.AddParameter("IsFire", AnimatorControllerParameterType.Bool);
            controller.AddParameter("FireRate", AnimatorControllerParameterType.Float);
        }
    }
}