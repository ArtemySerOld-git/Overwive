using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overwave.Classic.Editor
{
    public class AssetManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Overwave/Build Database")]
        public static void BuildConfigDatabase()
        {
            var guids = AssetDatabase.FindAssets("t:BaseConfig", new[] { "Assets" });

            var configs = guids.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BaseConfig>).Where(config => config).ToList();
            Debug.Log($"Found {configs.Count} configs");
            
            var db = CreateInstance<Assets.Database>();
            db.configs = configs.ToArray();
            AssetDatabase.CreateAsset(db, "Assets/Resources/Database.asset");
            AssetDatabase.SaveAssets();
        }
    }
}