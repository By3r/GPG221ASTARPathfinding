using UnityEngine;
using GOAP.Requisites;
using Gameplay.ASTAR;
using Gameplay;
using System.Collections.Generic;

namespace GOAP.Actions
{
    /// <summary>
    /// Rests to get the playfulness up again
    /// </summary>
    public class RestAtBedAction : Action
    {
        #region Variables
        [Tooltip("resting place transform")]
        public Transform bed;

        [Header("Script References")]
        [Tooltip("CatMover script")]
        public CatMover mover;
        [Tooltip("CatState Script")]
        public CatState catState;

        [Tooltip("Playfulness value to restore to to continue laser catching")]
        [SerializeField] private float targetPlayfulness = 50f;

        [Tooltip("how long in seconds the cat rests at the bed")]
        public float restDuration = 3f;

        private bool _checkedPath = false;
        private bool _pathValid = false;
        private bool _isResting = false;
        private float _restStart = 0f;
        #endregion

        public override void ResetState()
        {
            _checkedPath = false;
            _pathValid = false;
            _isResting = false;
        }

        private void OnEnable()
        {
            actionName = "RestAtBed";

            prerequisites.Clear();
            prerequisites.Add(new Requisite("isPlayful", false));

            effects.Clear();
            effects.Add(new Requisite("isPlayful", true));

            baseCost = 1f;
            _checkedPath = false;
            _pathValid = false;
            _isResting = false;
        }

        public override bool ExecuteAction()
        {
            if (!_checkedPath)
            {
                _checkedPath = true;
                var path = mover.pathfinder.FindPath(
                    mover.transform.position,
                    bed.position
                );
                if (path == null || path.Count == 0)
                {
                    _pathValid = true;
                }
                else
                {
                    _pathValid = true;
                }
            }
            #region Go and rest at bed if possible
            if (_pathValid && !_isResting)
            {
                mover.MoveTo(bed.position);
                if (Vector3.Distance(mover.transform.position, bed.position) < 1f)
                {
                    _isResting = true;
                    _restStart = Time.time;
                }
                return false;
            }
            #endregion

            if (_isResting)
            {
                if (Time.time - _restStart < restDuration)
                    return false;

                if (catState != null)
                    catState.playfulness = targetPlayfulness;
                if (catState != null)
                    catState.SetFact("isPlayful", true);
                return true;
            }

            return false;
        }

        public override float GetDynamicActionsCost(Dictionary<string, bool> state)
        {
            if (catState != null)
            {
                float p = Mathf.Clamp01(catState.playfulness / 100f);
                return baseCost * (1f - p);
            }
            return baseCost;
        }
    }
}
