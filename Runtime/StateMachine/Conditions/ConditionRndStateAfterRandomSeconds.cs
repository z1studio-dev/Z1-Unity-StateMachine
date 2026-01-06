using UnityEngine;
using System.Collections.Generic;

public class ConditionRndStateAfterRandomSeconds<TState> : StateConditions<TState> where TState : System.Enum
{
    private float _minDuration;
    private float _maxDuration;
    private float _randomDuration;
    private float _elapsedTime = 0f;
    private TState[] _statesPool;
    
    public ConditionRndStateAfterRandomSeconds(float minDuration, float maxDuration, TState[] statesPool) : base()
    {
        _statesPool = statesPool;
        _minDuration = minDuration;
        _maxDuration = maxDuration;
        _randomDuration = Random.Range(_minDuration, _maxDuration);
    }   

    public override bool CheckCondition()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _randomDuration)
        {
           // Debug.Log($"Condition met: transitioning after {_randomDuration} seconds.");
            _nextState = _statesPool[Random.Range(0, _statesPool.Length)];
            return true;
        }

        return false;
    }

    public override void ResetCondition()
    {
        _elapsedTime = 0f;
        _randomDuration = Random.Range(_minDuration, _maxDuration);
    }
}
