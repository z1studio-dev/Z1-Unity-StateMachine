using UnityEngine;

public class BabyAppaisePouceState : BabyInteractionState
{
    public BabyAppaisePouceState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Appaise_Pouce);
        SetBabyHeadFollow(false);
        SetBabyDampedRig(0f, 0.8f);
        Log.StateLog("Baby Appaise State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.AppaisePouce;
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Appaise_Pouce);
        SetBabyHeadFollow(false, duration: 1f);
        SetBabyDampedRig(0.5f, 0.8f);
        Log.StateLog("Baby Appaise State Exit");
    }
}
