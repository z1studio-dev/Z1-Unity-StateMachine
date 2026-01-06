using UnityEngine;
public class BabyDetresseFaintingState : BabyInteractionState
{
    public BabyDetresseFaintingState(BabyInteractionContext context) : base(context)
    {
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetressePresqueEvanoui;
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Detresse_PresqueEvanoui);
        // SetBabyScale(0.75f);
        
        Log.StateLog("Baby Fainting State Enter");
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_PresqueEvanoui);
        Log.StateLog("Baby Fainting State Exit");
    }
}
