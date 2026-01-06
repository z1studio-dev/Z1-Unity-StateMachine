using UnityEngine;

public class BabyDetressePLState : BabyInteractionState
{
    public BabyDetressePLState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Detresse_Boule);
     //   SetBabyScale(0.75f);
        
        Log.StateLog("Baby PLS State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetresseBoule;
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_Boule);
        Log.StateLog("Baby PLS State Exit");
    }

}
