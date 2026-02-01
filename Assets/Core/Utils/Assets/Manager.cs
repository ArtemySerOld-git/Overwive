using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Overwave.Assets
{
    public static class Manager
    {
        private static readonly Dictionary<string, BaseConfig> _configs = new();

        public static void LoadResources()
        {
            Database db = Resources.Load<Database>("Database");

            foreach (var config in db.configs)
            {
                if (!_configs.TryAdd(config.Id, config))
                    Debug.LogWarning($"Duplicate config ID: {config.Id}");
            }
            
            Debug.Log($"Loaded {_configs.Count} configs");
        }

        public static T Get<T>(string id) where T : BaseConfig
        {
            if (_configs.TryGetValue(id, out var config))
            {
                if (config is T typed) return typed;
                Debug.LogError($"Config '{id}' is not of type {typeof(T)}");
            }

            return null;
        }

        [CanBeNull] public static BaseConfig Get(string id)
            => _configs.GetValueOrDefault(id, null);

        public static T GetAny<T>(string id) where T : Object
        {
            if (_configs.TryGetValue(id, out var config))
            {
                if (config.Value is T typed) return typed;
                Debug.LogError($"Value of config '{id}' is not of type {typeof(T)}");
            }
            Debug.LogError($"Cannot find config '{id}'");
            return null;
        }
    }
}