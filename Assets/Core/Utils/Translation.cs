using System.Collections.Generic;
using UnityEngine;

namespace Overwave.Utils
{
    public static class Translation
    {
        private static int _currentLanguage;
        public static string[] Languages { get; private set; }
        
        public static Dictionary<string, TranslationData> Database { get; private set; }

        public static void Initialize(string[] languages)
        {
            Languages = languages;
            
            var db = Resources.Load<Assets.Database>("Database");
            Database = new Dictionary<string, TranslationData>();

            foreach (var config in db.configs)
            {
                if (!Database.TryAdd(config.Id, config.Translations))
                    Debug.LogWarning($"Duplicate ID: {config.Id}");
            }
        }

        public static string Get(string id)
        {
            if (Database.TryGetValue(id, out var translation))
                return translation[_currentLanguage];
            Debug.LogError($"Cannot find translation: {id}");
            return id;
        }

        public static void Switch(int newLanguage)
        {
            if (newLanguage >= Languages.Length)
            {
                Debug.LogWarning($"Language index {newLanguage} out of languages range");
                return;
            }
            
            _currentLanguage = newLanguage;
            Debug.Log($"Language switched to {newLanguage}");
        }

        public static void Switch(string newLanguage)
        {
            for (var i = 0; i < Languages.Length; i++)
            {
                if (Languages[i].Equals(newLanguage))
                {
                    _currentLanguage = i;
                    break;
                }
            }
            Debug.LogWarning($"Language {newLanguage} not contains in registered languages");
        }
    }
}