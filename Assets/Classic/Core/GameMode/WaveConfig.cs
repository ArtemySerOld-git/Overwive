using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave.Classic.GameMode
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Game Mode/Wave Config")]
    public class WaveConfig : ScriptableObject
    {
        [field: Tooltip("If value smaller 0, wave is infinity"), SerializeField]
        public int WaveTime { get; internal set; } = 60;
        [field: SerializeField] public ushort SkipTime { get; internal set; } = 20;
        [field: SerializeField] public ushort CashAfterWave { get; internal set; }

        [field: Space, SerializeField] public List<EnemyWaveConfig> Structure { get; internal set; } = new();
    }
}