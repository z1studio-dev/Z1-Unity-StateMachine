using UnityEngine;

public class BabyGoInsideBaleineState : BabyInteractionState
{
    public BabyGoInsideBaleineState(BabyInteractionContext context) : base(context)
    {
    }

    private float delayTimer = 0f;
    private bool scaleApplied = false;    
    public override void EnterState()
    {
        SetTransitionSpeed(0.2f);
        SetBabyAnimation(1f, AnimName.Appaise_Pouce);
        
        delayTimer = 0f;
        scaleApplied = false;
        
        Log.StateLog("Baby  Go Inside Baleine State Enter");
    }

    public override void UpdateState()
    {
        if (!scaleApplied)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= 2.5f)
            {
                SetBabyScale(10f);
                scaleApplied = true;
            }
        }

        Context.TransitionBabyScale();
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.GoInsideBaleine;
    }

    public override void ExitState()
    {
        Log.StateLog("Baby Go Inside Baleine State Exit");
    }
}
