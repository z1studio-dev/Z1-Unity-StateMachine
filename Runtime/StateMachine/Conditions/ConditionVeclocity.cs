using System;
using UnityEngine;
public class ConditionVeclocity<Estate> : StateConditions<Estate> where Estate : Enum
{
    private Vector2 _minMaxVelocity;
    private Rigidbody _rigidbody;

    public ConditionVeclocity(Estate nextState, Rigidbody rigidbody, Vector2 minMaxVelocity) : base(nextState)
    {
        _minMaxVelocity = minMaxVelocity;
        _nextState = nextState;
        _rigidbody = rigidbody;
    }


    public override bool CheckCondition()
    {
        float _velocity = _rigidbody.linearVelocity.magnitude;
        if(_velocity > _minMaxVelocity.x && _velocity < _minMaxVelocity.y)
        {
            return true;
        }else
        {
            return false;
        }
    }

    
}
