using UnityEngine;
using GOAP.Requisites;

namespace GOAP.Actions
{
    public class GoToDoorAction : Action
    {
        [Tooltip("Reference to the Door Transform.")]
        public Transform doorTransform;

        private void OnEnable()
        {
            actionName = "GoToDoor";

            prerequisites.Clear();
            prerequisites.Add(new Requisite("needToOpenDoor", true));
            prerequisites.Add(new Requisite("atDoor", false));

            effects.Clear();
            effects.Add(new Requisite("atDoor", true));

            baseCost = 1f;
        }

        public override bool ExecuteAction()
        {
            Debug.Log("[GoToDoor] (stub) Cat moves to the door.");
            return true;  
        }
    }
}
