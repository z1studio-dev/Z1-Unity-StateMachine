using UnityEngine;

public class ConditionDistance<TState> : StateConditions<TState> where TState : System.Enum
{
    private float _distance;
    private bool _isGreater;
    private Rigidbody _rigidbody;
    
    public ConditionDistance(TState nextState, Rigidbody rigidbody, float distance, bool isGreater = false) : base(nextState)
    {
        _distance = distance;
        _isGreater = isGreater;
        _rigidbody = rigidbody; 
    }

    public override bool CheckCondition()
    {
        float _distanceToPlayer = Vector3.Distance(_rigidbody.position, Camera.main.transform.position);
        if (_isGreater)
        {
            return _distanceToPlayer > _distance;
        }
        else
        {
            return _distanceToPlayer < _distance;
        }
    }
}
