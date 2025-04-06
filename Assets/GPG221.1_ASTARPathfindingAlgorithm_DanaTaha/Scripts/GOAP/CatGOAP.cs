using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP.Planner;
using GOAP.Actions;
using GOAP.WorldStates;
using Gameplay;
using Gameplay.ASTAR;

#region Goals ENUM
public enum GoalType
{
    ChaseLaser,
    OpenDoor,
    Meow,
    Rest
}
#endregion

public class CatGOAP : MonoBehaviour
{
    #region Public Variables
    [Tooltip("Is the cat smart?")]
    [SerializeField] private bool isSmart;

    [Header("References")]
    [SerializeField] private CatState catState;
    [SerializeField] private CatMover mover;
    [SerializeField] private LaserMover laserMover;

    private List<Action> availableActions = new List<Action>();

    [Header("Planner")]
    [SerializeField] private Planner planner;

    [Header("Current Goal")]
    public GoalType currentGoal = GoalType.ChaseLaser;

    [Header("Debug")]
    public List<string> lastPlan = new List<string>();
    public string currentAction = "";

    [SerializeField] private float goalTimeoutDuration = 5f;
    private float goalStartTime;

    private bool _planInProgress = false;
    #endregion

    private void Start()
    {
        #region Grab all actions from the holder of this script
        availableActions.AddRange(GetComponents<Action>());
        availableActions.AddRange(GetComponentsInChildren<Action>());
        #endregion

        foreach (var action in availableActions)
            action.ResetState();

        #region Event Subscriptions
        if (laserMover != null)
            laserMover.OnLaserMoved += _ =>
            {
                catState.SetFact("atLaserPoint", false);
                WorldState.Instance.SetFact("needToOpenDoor", false);
                catState.SetFact("didMeow", false);
                Replan();
            };

        WorldState.Instance.OnFactChanged += fact =>
        {
            if (fact == "doorOpen" || fact == "isNight")
                Replan();
        };

        catState.OnFactChanged += fact =>
        {
            switch (fact)
            {
                case "isSleepy":
                case "isPlayful":
                case "canMeow":
                case "needToOpenDoor":
                case "didMeow":
                case "atLaserPoint":
                    Replan();
                    break;
            }
        };
        #endregion
    }

    void Update()
    {
        #region Set goals based on Cat or World states
        if (catState.playfulness < 30f && !catState.GetFact("isPlayful"))
        {
            SetGoal(GoalType.Rest);
            return;
        }

        if (WorldState.Instance.GetFact("needToOpenDoor"))
        {
            SetGoal(isSmart ? GoalType.OpenDoor : GoalType.Meow);
            return;
        }

        if (catState.GetFact("canMeow") && catState.playfulness < 30f)
        {
            SetGoal(GoalType.Meow);
            return;
        }

        if (currentGoal == GoalType.Meow && catState.GetFact("didMeow"))
        {
            catState.SetFact("didMeow", false);
            SetGoal(GoalType.ChaseLaser);
            return;
        }

        if (currentGoal == GoalType.OpenDoor && WorldState.Instance.GetFact("doorOpen"))
        {
            SetGoal(GoalType.ChaseLaser);
            return;
        }

        if (currentGoal == GoalType.Rest && catState.GetFact("isPlayful"))
        {
            SetGoal(GoalType.ChaseLaser);
            return;
        }
        #endregion

        #region Grab a new goal to avoid being stuck on a previous goal for no reason
        if (Time.time - goalStartTime > goalTimeoutDuration)
        {
            if (catState.playfulness < 30f && !catState.GetFact("isPlayful"))
            {
                SetGoal(GoalType.Rest);
            }
            else if (WorldState.Instance.GetFact("needToOpenDoor"))
            {
                SetGoal(isSmart ? GoalType.OpenDoor : GoalType.Meow);
            }
            else if (catState.GetFact("canMeow") && catState.playfulness < 30f)
            {
                SetGoal(GoalType.Meow);
            }
            else
            {
                SetGoal(GoalType.ChaseLaser);
            }

            goalStartTime = Time.time;
        }
        #endregion
    }

    #region Private Functions
    private void SetGoal(GoalType newGoal)
    {
        if (currentGoal == newGoal) return;
        currentGoal = newGoal;
        goalStartTime = Time.time;
        Replan();
    }

    private void Replan()
    {
        foreach (var action in availableActions)
            action.ResetState();

        StopAllCoroutines();
        _planInProgress = false;
        StartNewPlan();
    }

    private void StartNewPlan()
    {
        if (_planInProgress) return;
        _planInProgress = true;
        StartCoroutine(RunPlan());
    }

    private Dictionary<string, bool> GetGoalEffects(GoalType goal)
    {
        var goalEffects = new Dictionary<string, bool>();
        switch (goal)
        {
            #region Get an action that eventually has the corresponding effect as true
            case GoalType.Rest:
                goalEffects["isPlayful"] = true;
                break;
            case GoalType.OpenDoor:
                goalEffects["doorOpen"] = true;
                break;
            case GoalType.Meow:
                goalEffects["didMeow"] = true;
                break;
            case GoalType.ChaseLaser:
            default:
                goalEffects["atLaserPoint"] = true;
                break;

            #endregion
        }
        return goalEffects;
    }

    private IEnumerator RunPlan()
    {
        var merged = new Dictionary<string, bool>(WorldState.Instance.facts);
        foreach (var kv in catState.facts)
            merged[kv.Key] = kv.Value;

        var goalMap = GetGoalEffects(currentGoal);

        lastPlan.Clear();

        var plan = planner.Plan(merged, goalMap, availableActions);

        if (plan == null || plan.Count == 0)
        {
            _planInProgress = false;
            yield break;
        }

        foreach (var a in plan)
            lastPlan.Add(a.actionName);

        foreach (var act in plan)
        {
            currentAction = act.actionName;
            bool done = false;

            while (!done)
            {
                done = act.ExecuteAction();
                yield return null;
            }

            foreach (var e in act.effects)
            {
                WorldState.Instance.SetFact(e.key, e.value);
            }
        }

        currentAction = "";
        _planInProgress = false;
    }
    #endregion
}
