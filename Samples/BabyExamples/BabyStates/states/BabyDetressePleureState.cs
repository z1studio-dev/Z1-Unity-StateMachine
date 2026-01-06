using UnityEngine;

public class BabyDetressePleureState : BabyInteractionState
{
    public BabyDetressePleureState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetTransitionSpeed(1f);
        SetBabyAnimation(1f, AnimName.Detresse_Pleure);
     //   SetBabyScale(0.75f);
        SetBabyHeadFollow(true, 0.2f, 2f);
        
      //  SetBabyScale(new Vector3(1f, 1f, 1f));
       // SetBabyColor(Color.red, "_Fresnel_color");
       // SetBabyColor(Color.red, "_Interior_color");
        Log.StateLog("Baby Crying State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.DetressePleure;
    }

    public override void ExitState()
    {
        SetBabyAnimation(0f, AnimName.Detresse_Pleure);
        SetBabyHeadFollow(false, duration: 1f);
        
        Log.StateLog("Baby Crying State Exit");
    }
}
