using UnityEngine;

public class ConditionAfterSeconds<TState> : StateConditions<TState> where TState : System.Enum
{
    private float _duration; // Time in seconds to wait before transitioning
    private float _elapsedTime = 0f; // Time elapsed since entering the state

    public ConditionAfterSeconds(TState nextState, float duration) : base(nextState)
    {
        _duration = duration;
    }

    public override bool CheckCondition()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _duration)
        {
            Debug.Log($"Condition met: transitioning after {_duration} seconds.");
            return true; // Transition to the next state
        }

        return false; // Stay in the current state
    }

    public override void ResetCondition()
    {
        _elapsedTime = 0f; // Reset the timer when entering a new state
    }
}
