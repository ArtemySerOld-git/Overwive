using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overwave.Classic.Editor
{
    public class AssetManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Overwave/Build Database")]
        private static void BuildConfigDatabase()
        {
            List<BaseConfig> configs = new();
            var guids = AssetDatabase.FindAssets("t:BaseConfig", new[] { "Assets" });

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                BaseConfig config = AssetDatabase.LoadAssetAtPath<BaseConfig>(path);
                if (config) configs.Add(config);
            }
            Debug.Log($"Found {configs.Count} configs");
            
            var db = CreateInstance<Assets.Database>();
            db.configs = configs.ToArray();
            AssetDatabase.CreateAsset(db, "Assets/Resources/Database.asset");
            AssetDatabase.SaveAssets();
        }
    }
}