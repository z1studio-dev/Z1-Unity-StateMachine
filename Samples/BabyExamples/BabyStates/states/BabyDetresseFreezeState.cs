using UnityEngine;

public class BabyDetresseFreezeState : BabyInteractionState
{
    public BabyDetresseFreezeState(BabyInteractionContext context) : base(context)
    {
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetresseFreeze;
    }

    public override void EnterState()
    {
        SetTransitionSpeed(2f);
      //  SetBabyScale(0.75f);
        SetBabySatAndLum(0.1f, 0.7f, 1f);
        SetBabyAnimation(1f, AnimName.Detresse_Freeze);
        SetBabyHeadFollow(true, 0.8f, 2f);
        Log.StateLog("Baby Freeze State Enter");
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_Freeze);
        Log.StateLog("Baby Freeze State Exit");
    }
}
