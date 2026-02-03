using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave
{
    public abstract class BaseConfig : ScriptableObject
    {
        [field: SerializeField]
        public string Id { get; internal set; }
        [field: SerializeField]
        public Utils.TranslationData Translations { get; internal set; }
        
        public abstract Object Value { get; }
    }
}