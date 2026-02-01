using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]

namespace Overwave.Classic.GameMode
{
    [CreateAssetMenu(menuName = "Overwave/Classic/Game Mode/Config", order = -1000)]
    public class GameModeConfig : BaseConfig
    {
        [field: Header("Reward"), SerializeField]
        public ushort CoinsReward { get; internal set; }
        [field: SerializeField]
        public ushort GemsReward { get; internal set; }
        [field: SerializeField]
        public ushort XpReward { get; internal set; }
        [field: SerializeField]
        public int StartCash { get; internal set; }
        
        [field: Space, SerializeField]
        public WaveConfig[] Structure { get; internal set; }

        public override Object Value => this;
    }
}