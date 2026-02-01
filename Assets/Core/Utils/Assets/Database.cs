using UnityEngine;

namespace Overwave.Assets
{
    [CreateAssetMenu(menuName = "Overwave/Asset Database", order = -1000)]
    public class Database : ScriptableObject
    {
        [field: SerializeField] public BaseConfig[] configs;
    }
}