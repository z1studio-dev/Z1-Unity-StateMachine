using UnityEngine;

public class BabyStateNone : BabyInteractionState
{
    public BabyStateNone(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.None;
    }

    public override void ExitState()
    {
    }
}
