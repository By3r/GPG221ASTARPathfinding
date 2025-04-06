using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Personal Cat states
    /// </summary>
    public class CatState : MonoBehaviour
    {
        #region Variables
        [Range(0, 100)] public float playfulness = 50f;
        public float playfulnessIncreaseRate = 2f;

        public Dictionary<string, bool> facts = new Dictionary<string, bool>();

        private int _version = 0;
        public event Action<string> OnFactChanged;
        public int GetVersion() => _version;
        #endregion

        private void Awake()
        {
            #region Set Facts
            facts.Clear();
            facts["atLaserPoint"] = false;
            facts["didMeow"] = false;
            facts["isPlayful"] = false;
            facts["isSleepy"] = false;
            facts["isNight"] = false;
            facts["atDoor"] = false;
            #endregion
        }

        private void Update()
        {
            playfulness = Mathf.Clamp(playfulness + playfulnessIncreaseRate * Time.deltaTime / 60f, 0f, 100f);
            if (playfulness == 100) { ResetPlayfulness(); }
            SetFact("isPlayful", playfulness >= 50f);
        }

        #region Public Functions
        public bool GetFact(string key) =>
            facts.TryGetValue(key, out bool v) && v;

        public void SetFact(string key, bool value)
        {
            if (facts.TryGetValue(key, out bool old) && old == value)
                return;
            facts[key] = value;
            _version++;
            OnFactChanged?.Invoke(key);
        }
        public void ResetPlayfulness() => playfulness = 0f;
        #endregion
    }
}
