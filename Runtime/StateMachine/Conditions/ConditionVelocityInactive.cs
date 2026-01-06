using System;
using UnityEngine;
public class ConditionVeclocityInactive<EState> : StateConditions<EState> where EState : Enum
{
    private float _minThreshold;
    private float _timeThreshold;
    private float _velocityUnderThresholdTime = 0f;
    private Rigidbody _rigidbody;

    public ConditionVeclocityInactive(EState nextState, Rigidbody rigidbody, float minThreshold, float timeThreshold): base(nextState)
    {
        _minThreshold = minThreshold;
        _timeThreshold = timeThreshold;
        _nextState = nextState;
        _rigidbody = rigidbody;
    }

    public override bool CheckCondition()
    {
        if (_rigidbody.linearVelocity.magnitude < _minThreshold)
        {
            _velocityUnderThresholdTime += Time.deltaTime;
            if (_velocityUnderThresholdTime >= _timeThreshold)
            {
                return true;
            }
        }
        else
        {
            _velocityUnderThresholdTime = 0f; // Reset the timer if velocity is above the threshold
        }

        return false;
    }
}

