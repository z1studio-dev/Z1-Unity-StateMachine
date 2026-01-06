using UnityEngine;

public class BabyDetresseInerteState : BabyInteractionState
{
    public BabyDetresseInerteState(BabyInteractionContext context) : base(context)
    {
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetresseInerte;
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Detresse_Inerte);
      //  SetBabyScale(0.75f);
        
        Log.StateLog("Baby Inerte State Enter");
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_Inerte);
        Log.StateLog("Baby Inerte State Exit");
    }
    
}
