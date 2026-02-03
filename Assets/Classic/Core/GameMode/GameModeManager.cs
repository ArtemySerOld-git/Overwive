using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Overwave.Classic.Utils;
using TMPro;
using UnityEngine;

namespace Overwave.Classic.GameMode
{
    public class GameModeManager : MonoBehaviour
    {
        private GameModeConfig _config;
        
        [field: SerializeField]
        public TMP_Text TimerText { get; private set; }
        [field: SerializeField]
        public TMP_Text WaveText { get; private set; }

        private readonly List<GameObject> _wavesEnd = new();

        public static bool WaveEnd;
        
        public static readonly System.Action WaveEndAction = delegate { };
        
        public static GameModeManager Instance { get; private set; }

        [SerializeField] private GameObject _miniWavePrefab;

        private void Start()
        {
            if (!Instance)
                Instance = this;
            if (Instance != this)
                Destroy(this);

            StartCoroutine(Play());
            
            _config = MainManager.Instance.gameModeConfig;
            
            GameObjectPool.LoadGameMode(_config);
        }

        public IEnumerator Play()
        {
            for (var timer = 10; timer >= 0; timer--)
            {
                TimerText.text = timer.ToString();
                yield return new WaitForSeconds(1);
            }

            for (var i = 0; i < _config.Structure.Count; i++)
            {
                WaveEnd = false;

                var wave = _config.Structure[i];
                foreach (var config in wave.Structure)
                {
                    var obj = Instantiate(_miniWavePrefab);
                    var manager = obj.GetComponent<EnemyWaveManager>();
                    
                    _wavesEnd.Add(obj);
                    manager.End += () => _wavesEnd.Remove(obj);
                    manager.Init(config);
                }
                
                WaveText.text = (i + 1).ToString();

                if (wave.WaveTime <= 0) TimerText.text = "∞";
                else
                {
                    for (var time = wave.WaveTime; time != 0; time--)
                    {
                        TimerText.text = time.ToString();
                        
                        yield return new WaitForSeconds(1);
                        if (WaveEnd) break;
                    }
                }
                
                Debug.Log($"Wave {i} ended");
                WaveEnd = true;
                WaveEndAction();
                for (var timer = 5; timer != 0; timer--)
                {
                    TimerText.text = timer.ToString();
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}