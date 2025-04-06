using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GOAP.Requisites;

namespace GOAP.Actions
{
    /// <summary>
    /// Base class for all GOAP actions.  
    /// Handles cost calculation, precondition checks, and execution flow.
    /// </summary>
    public abstract class Action : MonoBehaviour
    {
        #region Variables
        [Header("Action Settings")]
        public string actionName;
        [Tooltip("The cost completing this action")]
        public float baseCost = 1f;
        [Tooltip("Action requirements to perform")]
        public List<Requisite> prerequisites = new List<Requisite>();
        [Tooltip("Action performance result")]
        public List<Requisite> effects = new List<Requisite>();
        #endregion

        #region Public Functions

        #region Reset Action flags state
        public virtual void ResetState() { }
        #endregion

        #region Calculate the total cost and adds a penalty for each unmet prerequisite.
        /// <param name="worldState">Current values of world facts.</param>
        /// <returns>Amount of unmet prerequisites in addition to action's base cost.</returns>
        public virtual float GetDynamicActionsCost(Dictionary<string, bool> worldState)
        {
            int unmet = prerequisites.Count(r =>
                !worldState.TryGetValue(r.key, out bool val) || val != r.value
            );
            return baseCost + unmet;
        }
        #endregion

        #region Checks if all prerequisites are satisfied in the provided state of the world.
        /// <param name="worldState">The values of world facts.</param>
        /// <returns> True or Falase depending on the amount of met prerequisites.</returns>
        public bool ArePreconditionsMet(Dictionary<string, bool> worldState) => prerequisites.All(r => worldState.TryGetValue(r.key, out bool val) && val == r.value);
        #endregion

        #region Declare which agent performs an action and determine its completion.
        /// <param name="agent">The GameObject performing the action.</param>
        /// <param name="goapAgent">The GOAP controller driving this agent to perform the action.</param>
        /// <returns> True if the action has completed; false if it’s still running.</returns>
        public virtual bool Perform(GameObject agent, CatGOAP goapAgent) => ExecuteAction();
        #endregion

        #region Perform an action and return its status of completion
        /// <returns> Completion status of action.</returns>
        public abstract bool ExecuteAction();
        #endregion

        #region Update Action costs according to circumstances in the world.
        public virtual void UpdateActionCost() { }
        #endregion

        #endregion
    }
}