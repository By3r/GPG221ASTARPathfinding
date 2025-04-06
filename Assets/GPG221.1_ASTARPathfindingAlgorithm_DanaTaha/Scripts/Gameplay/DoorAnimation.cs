using Gameplay;
using GOAP.WorldStates;
using UnityEngine;

namespace GOAP.Actions
{
    public class DoorAnimation : MonoBehaviour
    {
        #region Variables
        [Tooltip("Door's status")]
        public bool IsOpen { get; private set; } = false;

        private Animator _animator;
        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            #region Manually close the door
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CloseDoor();
            }
            #endregion
        }

        #region Public Functions
        /// <summary>
        /// Invokes door open animation, sets the door's state as open and cats can't complain about it being closed.
        /// </summary>
        public void OpenDoor()
        {
            _animator?.SetBool("OpenDoor", true);
            IsOpen = true;

            WorldState.Instance.SetFact("doorOpen", true);
            WorldState.Instance.SetFact("canMeow", false);
        }

        /// <summary>
        /// Invokes door close animation, sets the door's state as close and cats can complain about it being closed.
        /// </summary>
        public void CloseDoor()
        {
            _animator?.SetBool("OpenDoor", false);
            IsOpen = false;

            WorldState.Instance.SetFact("doorOpen", false);
            WorldState.Instance.SetFact("canMeow", true);
        }
        #endregion
    }
}
