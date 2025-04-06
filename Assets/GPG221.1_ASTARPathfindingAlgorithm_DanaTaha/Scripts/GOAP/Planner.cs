using System.Collections.Generic;
using UnityEngine;

namespace GOAP.Planner
{
    /// <summary>
    /// Responsible for gathering available actions, compares costs to form a plan that fulfills a goal.
    /// </summary>
    public class Planner : MonoBehaviour
    {
        public List<GOAP.Actions.Action> Plan(Dictionary<string, bool> worldState, Dictionary<string, bool> goal, List<GOAP.Actions.Action> availableActions)
        {
            List<GOAP.Actions.Action> bestPlan = null;
            float bestCost = float.MaxValue;

            #region pushes plan node to a stack and tracks visited states using hashed string version of world states
            var stack = new Stack<PlanNode>();
            var visited = new HashSet<string>();
            stack.Push(new PlanNode(worldState, new List<GOAP.Actions.Action>(), 0f));
            visited.Add(GetStateKey(worldState));
            #endregion

            #region As long as the stack isn't empty, check if the goal is satisfied
            int iter = 0, maxIter = 10000;
            while (stack.Count > 0 && iter++ < maxIter)
            {
                var node = stack.Pop();
                #region if the goal is met and the cost was cheaper than any of the other plans, save the plan
                if (GoalSatisfied(node.state, goal))
                {
                    if (node.cost < bestCost)
                    {
                        bestCost = node.cost;
                        bestPlan = new List<GOAP.Actions.Action>(node.plan);
                    }
                    continue;
                }
                #endregion

                #region loop through all available actions to see what else can be done
                foreach (var action in availableActions)
                {
                    if (!action.ArePreconditionsMet(node.state))
                        continue;

                    var newState = new Dictionary<string, bool>(node.state);
                    foreach (var e in action.effects)
                        newState[e.key] = e.value;

                    float cost = node.cost + action.GetDynamicActionsCost(node.state);
                    if (cost >= bestCost)
                        continue;

                    string key = GetStateKey(newState);
                    if (visited.Contains(key))
                        continue;
                    visited.Add(key);

                    var plan = new List<GOAP.Actions.Action>(node.plan) { action };
                    stack.Push(new PlanNode(newState, plan, cost));
                }
                #endregion
            }
            #endregion

#if DEBUG_PLAN
                if (bestPlan == null) Debug.Log("[Planner Line 67] No valid plan.");
                else
                {
                    Debug.Log($"[Planner Line 70] Best plan (cost={bestCost}, steps={bestPlan.Count}):");
                    for (int i = 0; i < bestPlan.Count; i++)
                        Debug.Log($"    {i + 1}. {bestPlan[i].actionName}");
                }
#endif

            return bestPlan;
        }

        #region Private Functions
        private bool GoalSatisfied(Dictionary<string, bool> state, Dictionary<string, bool> goal)
        {
            foreach (var kv in goal)
                if (!state.TryGetValue(kv.Key, out bool v) || v != kv.Value)
                {
                    return false;
                }
            return true;
        }

        private string GetStateKey(Dictionary<string, bool> state)
        {
            var parts = new List<string>();
            foreach (var kv in state)
            {
                parts.Add($"{kv.Key}={kv.Value}");
            }
            parts.Sort();
            return string.Join(";", parts);
        }
        #endregion
    }

    #region Side Classe
    public class PlanNode
    {
        #region Variables
        public Dictionary<string, bool> state;
        public List<GOAP.Actions.Action> plan;
        public float cost;
        #endregion

        public PlanNode(Dictionary<string, bool> s, List<GOAP.Actions.Action> p, float c)
        {
            state = new Dictionary<string, bool>(s);
            plan = new List<GOAP.Actions.Action>(p);
            cost = c;
        }
    }
    #endregion
}
