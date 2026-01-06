using UnityEngine;

public class WhaleNageState : WhaleInteractionState
{
    public WhaleNageState(WhaleInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        Log.StateLog("Whale Nage State");
        SetWhaleAnimation(1, "Nage");
    }

    public override WhaleStateMachine.EWhaleState GetStateKey()
    {
        return WhaleStateMachine.EWhaleState.Nage;
    }

    public override void ExitState()
    {
        SetWhaleAnimation(0, "Nage");
    }

    public override void UpdateState()
    {
        UpdateWhale();
    }
}
