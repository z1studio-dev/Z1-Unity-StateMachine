using UnityEngine;

public class BabySouffleState : BabyInteractionState
{
    public BabySouffleState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Appaise_Pouce);
        DisplayToolTip(true, "SouffleToolTip");
        Log.StateLog("Baby Floating State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.Souffle;
    }

    public override void ExitState()
    {
        Context.HandAttractorTrack(false);
        DisplayToolTip(false, "SouffleToolTip");
        CountDownController.StartTimer(TimelineManager.Instance.PlayTimeline, 3f);
        Context.ArcMoverStart();
        Log.StateLog("Baby Floating State Exit");
    }
}
