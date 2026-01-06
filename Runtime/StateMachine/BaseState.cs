using System;
using UnityEngine.Assertions;
using System.Collections.Generic;

public abstract class BaseState<EState> where EState : Enum
{
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract bool TransitionToState();
    public abstract void ExitState();
    public abstract EState GetStateKey();
    
    public List<StateConditions<EState>> conditions;
    public Result<EState> GetNextStateifConditionMet(){
        Result<EState> res = new Result<EState>();
        res.stateChanged = false;
        res.nextState = GetStateKey();
        if(conditions == null || conditions.Count == 0){
            return res; // No conditions, no state change
        }
        foreach(StateConditions<EState> c in conditions){
            if(c.CheckCondition()){
                res.stateChanged = true;
                res.nextState = c.GetNextState();
                Log.StateLog($"State: {GetStateKey()} condition: {c.GetType().Name} met switching to: {c.GetNextState()}");
                break;
            }
        }
        return res;
    }

    public BaseState<EState> AddCondition(StateConditions<EState> condition){
        if(conditions == null){
            conditions = new List<StateConditions<EState>>();
        }
        conditions.Add(condition);
        return this;
    }

    // Reset all conditions when entering a new state (ex: reset timers)
    public void ResetConditions(){
        if(conditions == null || conditions.Count == 0)
        {
            return; // No conditions to reset
        }
        foreach (StateConditions<EState> c in conditions)
        {
            c.ResetCondition();
        }
    }
}

public class Result<Estate> where Estate : Enum
{
    public bool stateChanged;
    public Estate nextState;
}
