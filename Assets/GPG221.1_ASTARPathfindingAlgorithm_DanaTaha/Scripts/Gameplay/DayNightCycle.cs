using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP.WorldStates;

namespace Gameplay
{
    /// <summary>
    /// Simulates Day and Night by changing the Camera's solid background colour and tweaking Lights activity.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        #region Variables
        [Header("Time")]
        [Tooltip("Duration of the day in seconds")]
        public float dayDuration = 30f;

        [Tooltip("Duration of night  in seconds")]
        public float nightDuration = 10f;

        [Header("Background")]
        [Tooltip("Camera's background color for day time. By Default it's light blue.")]
        public Color dayBackgroundColor = new Color(0.529f, 0.808f, 0.922f);
        [Tooltip("Camera's background color for night time. By Default it's dark blue.")]
        public Color nightBackgroundColor = new Color(0.0f, 0.0f, 0.2f);

        [Header("Light")]
        [SerializeField] private List<GameObject> lights = new List<GameObject>();

        private Camera _mainCamera;
        #endregion

        private void Start()
        {
            _mainCamera = Camera.main;
            StartCoroutine(CycleRoutine());
        }

        private IEnumerator CycleRoutine()
        {
            while (true)
            {
                #region Day Routine
                WorldState.Instance.SetFact("isNight", false);

                if (_mainCamera != null)
                {
                    _mainCamera.backgroundColor = dayBackgroundColor;
                    SetLightActiveness(lights[0], lights[1]);
                }

                yield return new WaitForSeconds(dayDuration);
                #endregion

                #region Night Routine
                WorldState.Instance.SetFact("isNight", true);

                if (_mainCamera != null)
                {
                    _mainCamera.backgroundColor = nightBackgroundColor;
                    SetLightActiveness(lights[1], lights[0]);
                }

                yield return new WaitForSeconds(nightDuration);
                #endregion
            }
        }

        #region Private Function
        private void SetLightActiveness(GameObject activateLight, GameObject deactivateLight)
        {
            activateLight.SetActive(true);
            deactivateLight.SetActive(false);
        }
        #endregion
    }
}