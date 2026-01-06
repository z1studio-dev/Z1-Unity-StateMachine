using System;
using UnityEngine;

/// <summary>
/// Only becomes true if the chosen hand‐(or‐both) speed has stayed between
/// _minMaxVelocity.x and _minMaxVelocity.y for at least _requiredDuration seconds.
/// </summary>
public class ConditionHandsVeclocity<EState> : StateConditions<EState> where EState : Enum
{
    private readonly Vector2 _minMaxVelocity;    // [minSpeed, maxSpeed]
    private readonly HandsSelector _handsSelection;
    private readonly float _requiredDuration;    // how many continuous seconds in‐range before CheckCondition() returns true

    // Tracks how many seconds the velocity has continuously stayed in [min, max].
    // Resets to 0 as soon as velocity falls outside the window.
    private float _elapsedInRange = 0f;

    public ConditionHandsVeclocity(
        EState nextState,
        HandsSelector handsSelection,
        Vector2 minMaxVelocity, 
        float requiredDuration
    ) : base(nextState)
    {
        _minMaxVelocity   = minMaxVelocity;
        _handsSelection   = handsSelection;
        _requiredDuration = requiredDuration;
        _nextState        = nextState;
    }

    /// <summary>
    /// Called every frame by the state machine. We accumulate Time.deltaTime
    /// only while velocity is within [_minMaxVelocity.x, _minMaxVelocity.y]. 
    /// If it ever goes out of that range, we zero out _elapsedInRange.
    /// Once _elapsedInRange ≥ _requiredDuration, we return true.
    /// </summary>
    public override bool CheckCondition()
    {
        // 1) Sample the current scalar speed based on left/right/both
        float speed;
        switch (_handsSelection)
        {
            case HandsSelector.Left:
                speed = HandVelocityUtility.GetLeftHandSpeed();
                break;
            case HandsSelector.Right:
                speed = HandVelocityUtility.GetRightHandSpeed();
                break;
            case HandsSelector.Both:
                speed = HandVelocityUtility.GetBothHandsSpeed();
                break;
            case HandsSelector.Dual:
                speed = Mathf.Max(HandVelocityUtility.GetLeftHandSpeed(), HandVelocityUtility.GetRightHandSpeed());
                break;
            default:
                speed = 0f;
                break;
        }

        // 2) Is it in the desired range?
        bool inRange = (speed >= _minMaxVelocity.x) && (speed <= _minMaxVelocity.y);
        //Log.StateLog($"ConditionHandsVeclocity {_nextState}: speed={speed}, inRange={inRange}, elapsedInRange={_elapsedInRange}");
        if (inRange)
        {
            // Accumulate how long we've been continuously in range
            _elapsedInRange += Time.deltaTime;

            // If we've now met or exceeded the required duration, signal true
            if (_elapsedInRange >= _requiredDuration)
            {
                return true;
            }
            else
            {
                return false; // still counting up
            }
        }
        else
        {
            // Fell outside the window → reset timer
            _elapsedInRange = 0f;
            return false;
        }
    }

    /// <summary>
    /// Whenever the state machine re‐enters a state, it should call ResetCondition()
    /// on each attached condition. We clear our accumulated time here so that
    /// the “in‐range” counting always starts fresh.
    /// </summary>
    public override void ResetCondition()
    {
        _elapsedInRange = 0f;
    }
}
