using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.WorldStates
{
    /// <summary>
    /// World facts shared by alll cats.
    /// </summary>
    public class WorldState : MonoBehaviour
    {
        #region Variables
        public static WorldState Instance { get; private set; }

        public Dictionary<string, bool> facts = new Dictionary<string, bool>();

        public event Action<string> OnFactChanged;

        [SerializeField] private float canMeowDuration = 5f;
        [SerializeField] private float canMeowCooldown = 10f;
        #endregion

        private void Start()
        {
            MeowCycle();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                facts.Clear();
                facts["doorOpen"] = false;
                facts["isNight"] = false;
                facts["canMeow"] = true;
                facts["needToOpenDoor"] = false;
            }
            else Destroy(gameObject);
        }

        #region Public Functions
        public bool GetFact(string key) =>
            facts.TryGetValue(key, out bool v) && v;

        public void SetFact(string key, bool value)
        {
            if (facts.TryGetValue(key, out bool old) && old == value)
                return;
            facts[key] = value;
            OnFactChanged?.Invoke(key);
        }
        #endregion

        private IEnumerator MeowCycle()
        {
            while (true)
            {
                SetFact("canMeow", true);
                yield return new WaitForSeconds(canMeowDuration);
                SetFact("canMeow", false);
                yield return new WaitForSeconds(canMeowCooldown);
            }
        }
    }
}
