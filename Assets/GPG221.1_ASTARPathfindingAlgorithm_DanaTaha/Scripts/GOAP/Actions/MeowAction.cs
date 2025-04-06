using UnityEngine;
using GOAP.Requisites;
using GOAP.WorldStates;
using Gameplay;
using System.Collections.Generic;

namespace GOAP.Actions
{
    public class MeowAction : Action
    {
        #region Variables
        [Tooltip("CatState script")]
        public CatState catState;
        #endregion

        public override void ResetState() { }

        private void OnEnable()
        {
            actionName = "Meow";

            prerequisites.Clear();
            prerequisites.Add(new Requisite("canMeow", true));

            effects.Clear();
            effects.Add(new Requisite("didMeow", true));
            effects.Add(new Requisite("canMeow", false));

            baseCost = 1f;
        }

        #region Public Functions
        public override bool ExecuteAction()
        {
            Debug.Log("Meoowww");
            catState.playfulness = 0f;
            return true;
        }

        public override float GetDynamicActionsCost(Dictionary<string, bool> state)
        {
            return baseCost;
        }
        #endregion
    }
}
