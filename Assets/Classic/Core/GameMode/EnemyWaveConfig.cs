using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave.Classic.GameMode
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Game Mode/Enemy Wave Config")]
    public class EnemyWaveConfig : ScriptableObject
    {
        [field: SerializeField]
        public Enemy.Config Config { get; internal set; }
        [field: SerializeField]
        public int Count { get; internal set; }
        [field: SerializeField]
        public float Interval { get; internal set; }
        [field: SerializeField]
        public float StartDelay { get; internal set; }
    }
}