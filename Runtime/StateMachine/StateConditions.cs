using System;

public abstract class StateConditions<Estate> where Estate : Enum
{  
    public abstract bool CheckCondition();

    protected Estate _nextState;
    public Estate GetNextState() => _nextState;

    public StateConditions() { }

    public StateConditions(Estate nextState)
    {
        _nextState = nextState;
    }

    public virtual void ConditionEnter() { }
    public virtual void ConditionExit() { }


    public virtual void ResetCondition()
    {
        // Reset the condition when entering a new state
    }
}
