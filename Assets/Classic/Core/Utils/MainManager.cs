using Overwave.Classic.GameMode;
using Overwave.Utils;
using UnityEngine;

namespace Overwave.Classic.Utils
{
    public class MainManager : MonoBehaviour
    {
        [Header("Portals")]
        public GameObject startPortal;
        public GameObject endPortal;
        
        [Header("Configs")]
        public GameModeConfig gameModeConfig;
        
        [Header("Scripts")]
        public PointMarker pointMarker;

        [SerializeField] private Tower.Config _testTower;
        
        public static MainManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            if (Instance != this)
                Destroy(this);

            if (!startPortal)
                startPortal = GameObject.FindGameObjectWithTag("Start Portal");
            if (!endPortal)
                endPortal = GameObject.FindGameObjectWithTag("End Portal");
            
            Assets.Manager.LoadResources();
            Translation.Initialize(new[] { "ru", "en", "fr", "ge", "ch" });
            
            GameObjectPool.LoadTower(_testTower);
        }
    }
}