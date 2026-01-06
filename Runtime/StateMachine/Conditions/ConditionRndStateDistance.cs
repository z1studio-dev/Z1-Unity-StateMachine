using UnityEngine;

public class ConditionRndStateDistance<TState> : StateConditions<TState> where TState : System.Enum
{
    private float _distance;
    private bool _isGreater;
    private Rigidbody _rigidbody;
    private TState[] _statesPool;
    
    public ConditionRndStateDistance(TState[] statePool, Rigidbody rigidbody, float distance, bool isGreater = false) : base()
    {
        _statesPool = statePool;    
        _distance = distance;
        _isGreater = isGreater;
        _rigidbody = rigidbody; 
    }

    public override bool CheckCondition()
    {
        float distanceToPlayer = CalculateDistanceToPlayer();
        bool conditionMet = _isGreater ? distanceToPlayer > _distance : distanceToPlayer < _distance;
        
        if (conditionMet)
        {
            SetRandomNextState();
        }
        
        return conditionMet;
    }

    private float CalculateDistanceToPlayer()
    {
        return Vector3.Distance(_rigidbody.position, Camera.main.transform.position);
    }

    private void SetRandomNextState()
    {
        _nextState = _statesPool[Random.Range(0, _statesPool.Length)];
    }
}