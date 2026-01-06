using System;
using UnityEngine;
public class ConditionTouch<EState> : StateConditions<EState> where EState : Enum
{

    public bool IsTouching { get; private set; } = false;
    public float requiredDuration = 0f;
    private float elapsedTouchTime = 0f;
    public ConditionTouch(EState nextState, float requiredDuration) : base(nextState)
    {
        _nextState = nextState;
        this.requiredDuration = requiredDuration;
    }

    public override void ConditionEnter()
    {
        HandCollisionManager.OnHandCollisionEvent += OnHandCollide;
        Debug.Log("ConditionTouch Entered");
    }

    public override void ConditionExit()
    {
        HandCollisionManager.OnHandCollisionEvent -= OnHandCollide;
        Debug.Log("ConditionTouch Exited");
    }

    private void OnHandCollide(ColliderJointEvent collision, Vector3 velocity)
    {
        if (collision.triggerState == TriggerState.Enter || collision.triggerState == TriggerState.Stay)
        {
            IsTouching = true;
            Debug.Log("ConditionTouch Detected Touch at: " + collision.pointOfContact);
        }
        else
        {
            IsTouching = false;
        }
    }

    public override bool CheckCondition()
    {
        if (IsTouching)
        {
            elapsedTouchTime += Time.deltaTime;
            if (elapsedTouchTime >= requiredDuration)
            {
                Debug.Log("ConditionTouch Detected true!");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            elapsedTouchTime = 0f; // Reset if not touching
            return false;
        }
    }
}
