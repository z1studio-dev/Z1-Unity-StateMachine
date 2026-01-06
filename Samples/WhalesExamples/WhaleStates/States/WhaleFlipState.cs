using UnityEngine;

public class WhaleFlipState : WhaleInteractionState
{
    public WhaleFlipState(WhaleInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        Log.StateLog("Whale Flip State");
        SetWhaleAnimation(1, "Flip");
    }

    public override WhaleStateMachine.EWhaleState GetStateKey()
    {
        return WhaleStateMachine.EWhaleState.Flip;
    }

    public override void ExitState()
    {
        SetWhaleAnimation(0, "Flip");
    }

    public override void UpdateState()
    {
        UpdateWhale();
    }
}
