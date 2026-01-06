using UnityEngine;

public class BabyFloatingState : BabyInteractionState
{
    public BabyFloatingState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetTransitionSpeed(.5f);
        SetBabyAnimation(1f, AnimName.Detresse_Inerte);
     //   SetBabyScale(1f);
        
        //SetBabyLayerWeight(1f, LayerName.FloatingLayer);
        SetBabyUpDownMouvement(true);
        Log.StateLog("Baby Floating State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.Floating;
    }

    public override void ExitState()
    {
        SetTransitionSpeed(2f);
        SetBabyAnimation(0f, AnimName.Detresse_Inerte);
        //SetBabyLayerWeight(0f, LayerName.FloatingLayer);
        SetBabyUpDownMouvement(false);
        //SetBabySatAndLum(1f, 1f, 3f);
        DisplayToolTip(false, "BabyHoldHands");
        //CountDownController.StartTimer(TimelineManager.Instance.PlayTimeline, 10f);
        Log.StateLog("Baby Floating State Exit");
    }
}
