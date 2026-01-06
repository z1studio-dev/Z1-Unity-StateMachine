using UnityEngine;

public class BabyDetressePetitMouvement : BabyInteractionState
{
    
    public BabyDetressePetitMouvement(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Detresse_PetitMouvement);
      //  SetBabyScale(0.75f);
        
        Log.StateLog("Baby Petit Mouvement State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetressePetitMouvement;
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_PetitMouvement);
        Log.StateLog("Baby Mouvement State Exit");
    }
    
}
