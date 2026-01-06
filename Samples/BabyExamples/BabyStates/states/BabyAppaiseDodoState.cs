using UnityEngine;

public class BabyAppaiseDodoState : BabyInteractionState
{
    public BabyAppaiseDodoState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        SetBabyAnimation(1f, AnimName.Appaise_Pouce);
        // SetBabyScale(0.5f);
        SetBabyHeadFollow(false);
        SetBabyDampedRig(0f, 2f);
        CountDownController.StartTimer(TimelineManager.Instance.PlayTimeline, 3f);
        Log.StateLog("Baby Appaise State Enter");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.AppaiseDodo;
    }

    public override void ExitState()
    {
        //SetBabyAnimation(0f, AnimName.Appaise_Pouce);
        //SetBabyDampedRig(0.5f, 2f);
        Log.StateLog("Baby Appaise State Exit");
    }
}
