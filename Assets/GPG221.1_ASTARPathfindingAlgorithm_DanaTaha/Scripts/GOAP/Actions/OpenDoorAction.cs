using UnityEngine;
using GOAP.Requisites;

namespace GOAP.Actions
{
    public class OpenDoorAction : Action
    {
        [Tooltip("DoorAnimation script.")]
        public DoorAnimation doorAnimation;

        private void OnEnable()
        {
            actionName = "OpenDoor";
            prerequisites.Clear();
            prerequisites.Add(new Requisite("needToOpenDoor", true));
            prerequisites.Add(new Requisite("atDoor", true));

            effects.Clear();
            effects.Add(new Requisite("doorOpen", true));
            effects.Add(new Requisite("needToOpenDoor", false));

            baseCost = 1f;
        }

        public override bool ExecuteAction()
        {
            if (doorAnimation == null)
            {
                Debug.LogError("OpenDoorAction: missing DoorAnimation!");
                return true;
            }

            doorAnimation.OpenDoor();
            return true;
        }
    }
}