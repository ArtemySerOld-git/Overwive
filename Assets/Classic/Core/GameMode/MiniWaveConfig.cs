using UnityEngine;

namespace Overwave.Classic.GameMode
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Game Mode/Mini Wave Config")]
    public class MiniWaveConfig : ScriptableObject
    {
        [field: SerializeField]
        public Enemy.Config Config { get; private set; }
        [field: SerializeField]
        public int Count { get; private set; }
        [field: SerializeField]
        public float Interval { get; private set; }
        [field: SerializeField]
        public float StartDelay { get; private set; }
    }
}