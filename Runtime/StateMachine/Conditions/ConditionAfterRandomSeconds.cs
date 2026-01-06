using UnityEngine;

public class ConditionAfterRandomSeconds<TState> : StateConditions<TState> where TState : System.Enum
{
    private float _minDuration;
    private float _maxDuration;
    private float _randomDuration;
    private float _elapsedTime = 0f;

    public ConditionAfterRandomSeconds(TState nextState, float minDuration, float maxDuration) : base(nextState)
    {
        _minDuration = minDuration;
        _maxDuration = maxDuration;
        _randomDuration = Random.Range(_minDuration, _maxDuration);
    }

    public override bool CheckCondition()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _randomDuration)
        {
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
