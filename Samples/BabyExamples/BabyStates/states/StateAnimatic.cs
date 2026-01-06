using UnityEngine;

public class BabyStateAnimaticSC2A : BabyInteractionState
{
    public BabyStateAnimaticSC2A(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        //Context.LockLocalTransform(true);
        SetBabyAnimation(1f, AnimName.Detresse_Inerte);
        SetBabyLayerWeight(1f, "SC2A_");
        SetBabySatAndLum(0f, 0.6f, 0f);

      //  SetBabyScale(1f);
        
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.CinematicSC2A;
    }

    public override void ExitState()
    {
        //Context.LockLocalTransform(false);
        SetBabyAnimation(0f, AnimName.Detresse_Inerte);
        SetBabyLayerWeight(0f, "SC2A_");
        
    }
}
