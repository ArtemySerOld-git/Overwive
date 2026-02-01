using System;
using System.IO;
using Overwave.Classic.GameMode;
using UnityEditor;
using UnityEngine;

namespace Overwave.Classic.Editor.GameMode
{
    public class GameModeCreatorWindow : EditorWindow
    {
        private string _name;
        
        [MenuItem("Tools/Overwave/Classic/Game Mode Creator")]
        private static void ShowWindow()
        {
            var window = GetWindow<GameModeCreatorWindow>("Game Mode Creator");
            window._name = "";
            window.Show();
        }

        private void OnGUI()
        {
            _name = EditorGUILayout.TextField("Game Mode name", _name);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Build"))
                Build();
        }

        private void Build()
        {
            var gmConfig = CreateInstance<GameModeConfig>();

            gmConfig.Id = "classic.gamemode." + _name.ToLower();

            var path = $"Assets/Classic/Content/GameModes/{_name}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                
            AssetDatabase.CreateAsset(gmConfig, $"{path}/{_name}.asset");
            
            Close();
        }
    }
}