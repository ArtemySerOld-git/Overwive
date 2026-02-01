using UnityEngine;

namespace Overwave.Classic.GameMode
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Game Mode/Wave Config")]
    public class WaveConfig : ScriptableObject
    {
        [field: Tooltip("If value smaller 0, wave is infinity"), SerializeField]
        public int WaveTime { get; private set; }
        [field: SerializeField]
        public ushort SkipTime { get; private set; }
        [field: SerializeField]
        public ushort CashAfterWave { get; private set; }
        
        [field: Space, SerializeField]
        public MiniWaveConfig[] Structure { get; private set; }
    }
}