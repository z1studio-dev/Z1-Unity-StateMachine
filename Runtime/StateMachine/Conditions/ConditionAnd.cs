using System;
using System.Collections.Generic;

/// <summary>
/// A composite condition: returns true only when ALL of its inner StateConditions are true.
/// </summary>
/// <typeparam name="Estate">Your enum of possible states.</typeparam>
public class ConditionAnd<Estate> : StateConditions<Estate> where Estate : Enum
{
    // List of sub‐conditions to “AND” together
    private readonly List<StateConditions<Estate>> _subConditions = new List<StateConditions<Estate>>();

    /// <summary>
    /// Create a new AND‐condition. As soon as ALL supplied conditions return true,
    /// this composite’s Check() returns true and GetNextState() will be returned by your state machine.
    /// </summary>
    /// <param name="nextState">
    ///   The state to transition to if (and only if) all sub‐conditions are satisfied.
    /// </param>
    /// <param name="conditions">
    ///   One or more StateConditions&lt;Estate&gt; instances to AND together.
    ///   Each sub‐condition must have the same NextState (or you can ignore its own NextState internally).
    /// </param>
    public ConditionAnd(Estate nextState, params StateConditions<Estate>[] conditions)
        : base(nextState)
    {
        // Copy them into our internal list
        _subConditions.AddRange(conditions);
    }

    /// <summary>
    /// Returns true only if every sub‐condition.Check() is true.
    /// </summary>
    public override bool CheckCondition()
    {
        foreach (var cond in _subConditions)
        {
            if (!cond.CheckCondition())
            {
                return false;
            }
        }
        Log.StateLog($"ConditionAnd: All sub-conditions met for state {_nextState}");
        return true;
    }


    public override void ResetCondition()
    {
        foreach (var cond in _subConditions)
            cond.ResetCondition();
    }
}
