using System.Collections.Generic;
using System.IO;
using Overwave.Classic.GameMode;
using UnityEditor;
using UnityEngine;

namespace Overwave.Classic.Editor.GameMode
{
    public class GameModeCreatorWindow : EditorWindow
    {
        private class EnemyWaveConfigWithFoldout
        {
            public Classic.Enemy.Config config;
            public int count;
            public float interval;
            public float startDelay;
            public bool foldout = true;

            public EnemyWaveConfig ToDefaultConfig(string gameModeName, string outputFolder, int index)
            {
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
                
                var cfg = CreateInstance<EnemyWaveConfig>();
                cfg.Config = config;
                cfg.Count = count;
                cfg.Interval = interval;
                cfg.StartDelay = startDelay;
                
                AssetDatabase.CreateAsset(cfg, Path.Combine(outputFolder,
                    $"{gameModeName}_Wave{index}_{config.name.Replace("Enemy_", "").Replace("_Config", "")}.asset"));
                
                return cfg;
            }
        }
        
        private class WaveConfigWithFoldout
        {
            public int time = 60;
            public ushort skipTime = 20;
            public ushort cashAfterWave;
            public readonly List<EnemyWaveConfigWithFoldout> structure = new();
            public bool structureFoldout = true;
            public bool foldout = true;

            public WaveConfig ToDefaultConfig(string gameModeName, string outputFolder, int index)
            {
                outputFolder = Path.Combine(outputFolder, $"Wave {index}");
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
                
                var cfg = CreateInstance<WaveConfig>();
                cfg.WaveTime = time;
                cfg.SkipTime = skipTime;
                cfg.CashAfterWave = cashAfterWave;
                foreach (var wave in structure)
                    cfg.Structure.Add(wave.ToDefaultConfig(gameModeName, outputFolder, index));
                
                AssetDatabase.CreateAsset(cfg, Path.Combine(outputFolder, $"{gameModeName}_Wave{index}.asset"));
                
                return cfg;
            }
        }

        private class GameModeConfigWithFoldout
        {
            public string name;
            public ushort coinsReward;
            public ushort gemsReward;
            public ushort xpReward;
            public ushort startCash;
            public List<WaveConfigWithFoldout> structure = new();
            public bool foldout;

            public GameModeConfig ToDefaultConfig(string outputFolder)
            {
                var cfg = CreateInstance<GameModeConfig>();
                cfg.Id = "classic.gamemode." + name.Replace(" ", "_").ToLower();
                cfg.CoinsReward = coinsReward;
                cfg.GemsReward = gemsReward;
                cfg.XpReward = xpReward;
                cfg.StartCash = startCash;
                for (var i = 0; i < structure.Count; i++)
                    cfg.Structure.Add(structure[i].ToDefaultConfig(name, outputFolder, i));
                return cfg;
            }
        }
        
        private readonly GameModeConfigWithFoldout _config;
        private Vector2 _scroll;

        public GameModeCreatorWindow()
        {
            _config = new GameModeConfigWithFoldout();
            _config.structure.Add(new WaveConfigWithFoldout());
        }
        
        [MenuItem("Tools/Overwave/Classic/Game Mode Creator")]
        private static void ShowWindow()
        {
            var window = GetWindow<GameModeCreatorWindow>("Game Mode Creator");
            window.Show();
        }

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            
            _config.name = EditorGUILayout.TextField("Game Mode name", _config.name);
            
            EditorGUILayout.Space();

            _config.coinsReward  = (ushort)EditorGUILayout.IntField("Coins Reward", _config.coinsReward);
            _config.gemsReward   = (ushort)EditorGUILayout.IntField("Gems Reward", _config.gemsReward);
            _config.xpReward     = (ushort)EditorGUILayout.IntField("Xp Reward", _config.xpReward);
            _config.startCash    = (ushort)EditorGUILayout.IntField("Start Cash", _config.startCash);
            
            EditorGUILayout.Space();
            
            _config.foldout = EditorGUILayout.Foldout(_config.foldout, "Structure");
            if (_config.foldout)
            {
                EditorGUI.indentLevel++;
                
                for (var i = 0; i < _config.structure.Count; i++)
                    DrawWave(_config.structure[i], i);

                EditorGUILayout.Space();
            
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(EditorGUI.indentLevel * 15);
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.MaxWidth(60));
                    {
                        if (GUILayout.Button("+", GUILayout.MaxWidth(25)))
                            _config.structure.Add(new WaveConfigWithFoldout());
                        if (GUILayout.Button("-", GUILayout.MaxWidth(25)))
                            _config.structure.Remove(_config.structure[^1]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Build"))
                Build();
            
            EditorGUILayout.EndScrollView();
        }

        private static void DrawWave(WaveConfigWithFoldout wave, int index)
        {
            wave.foldout = EditorGUILayout.Foldout(wave.foldout, "Wave " + index);
            if (!wave.foldout) return;
            
            EditorGUI.indentLevel++;
            
            wave.time = EditorGUILayout.IntField("Time", wave.time);
            wave.skipTime = (ushort)EditorGUILayout.IntField("Skip Time", wave.skipTime);
            wave.cashAfterWave = (ushort)EditorGUILayout.IntField("Cash After Wave", wave.cashAfterWave);
            
            GUILayout.Space(15);

            wave.structureFoldout = EditorGUILayout.Foldout(wave.structureFoldout, "Structure");
            if (wave.structureFoldout)
            {
                EditorGUI.indentLevel++;
                
                for (var i = 0; i < wave.structure.Count; i++)
                    DrawEnemyWave(wave.structure[i], i);
            
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(EditorGUI.indentLevel * 15);
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.MaxWidth(60));
                    {
                        if (GUILayout.Button("+", GUILayout.MaxWidth(25)))
                            wave.structure.Add(new EnemyWaveConfigWithFoldout());
                        if (GUILayout.Button("-", GUILayout.MaxWidth(25)))
                            wave.structure.Remove(wave.structure[^1]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUI.indentLevel--;
            }
            
            GUILayout.Space(15);
            
            EditorGUI.indentLevel--;
        }

        private static void DrawEnemyWave(EnemyWaveConfigWithFoldout wave, int index)
        {
            wave.foldout = EditorGUILayout.Foldout(wave.foldout, "Enemy Wave " + index);
            if (!wave.foldout) return;

            EditorGUI.indentLevel++;

            wave.config = (Classic.Enemy.Config)EditorGUILayout.ObjectField("Config", wave.config,
                typeof(Classic.Enemy.Config), false);
            wave.count = EditorGUILayout.IntField("Count", wave.count);
            wave.interval = EditorGUILayout.FloatField("Interval", wave.interval);
            wave.startDelay = EditorGUILayout.FloatField("Start Delay", wave.startDelay);
            
            EditorGUI.indentLevel--;
        }

        private void Build()
        {
            var path = $"Assets/Classic/Content/GameModes/{_config.name}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                
            AssetDatabase.CreateAsset(_config.ToDefaultConfig(path), Path.Combine(path, $"{_config.name}.asset"));
            
            AssetManagerWindow.BuildConfigDatabase();
            
            Close();
        }
    }
}