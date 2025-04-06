using System.Collections.Generic;
using UnityEngine;
using GOAP.Requisites;
using GOAP.WorldStates;
using Gameplay;
using Gameplay.ASTAR;

namespace GOAP.Actions
{
    /// <summary>
    /// Moves the cat toward the laser point using ASTAR pathfinding and handles blocked paths by flagging door openings.
    /// </summary>
    public class ReachLaserAction : Action
    {
        #region Variables
        [Tooltip("Laser gameobject's transform component")]
        public Transform laser;

        [Header("Script References")]
        [Tooltip("CatMover script.")]
        public CatMover mover;

        [Tooltip("CatState script.")]
        public CatState catState;

        private bool _checkedPath;
        private bool _pathValid;
        #endregion

        public override void ResetState()
        {
            _checkedPath = false;
            _pathValid = false;
        }

        private void OnEnable()
        {
            actionName = "ReachLaser";
            prerequisites.Clear();
            prerequisites.Add(new Requisite("atLaserPoint", false));
            prerequisites.Add(new Requisite("isPlayful", true));

            effects.Clear();
            effects.Add(new Requisite("atLaserPoint", true));

            baseCost = 1f;

            ResetState();
        }

        public override bool ExecuteAction()
        {
            if (!_checkedPath)
            {
                _checkedPath = true;
                var path = mover.pathfinder.FindPath(mover.transform.position, laser.position);

                #region Check for a blocked path
                if (path == null || path.Count == 0)
                {
                    WorldState.Instance.SetFact("needToOpenDoor", true);
                    return true;
                }
                #endregion

                _pathValid = true;
            }

            #region If path is valid, move toward the laser
            if (_pathValid)
            {
                mover.MoveTo(laser.position);

                if (Vector3.Distance(mover.transform.position, laser.position) < 2f)
                {
                    return true;
                }
            }
            #endregion

            return false;
        }

        public override float GetDynamicActionsCost(Dictionary<string, bool> worldState)
        {
            float playFactor = 0f;

            if (catState != null)
            {
                playFactor = Mathf.Clamp01(catState.playfulness / 100f);
            }

            return 1f + (1f - playFactor) * 3f;
        }
    }
}