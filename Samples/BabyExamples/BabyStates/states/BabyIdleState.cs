using UnityEngine;

public class BabyIdleState : BabyInteractionState
{
    public BabyIdleState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetTransitionSpeed(2f);
        SetBabyAnimation(1f, AnimName.BabyIdle);
     //   SetBabyScale(0.5f);
        
        Log.StateLog("Baby Idle State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.Idle;
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.BabyIdle);
        Log.StateLog("Baby Idle State Exit");
    }
}
