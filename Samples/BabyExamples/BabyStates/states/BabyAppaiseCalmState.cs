using UnityEngine;
using System.Threading.Tasks;

public class BabyAppaiseCalmState : BabyInteractionState
{

    private bool stateIsActive = false;

    public BabyAppaiseCalmState(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {

        SetTransitionSpeed(2f);
        //SetBabyAnimation(1f, AnimName.Appaise_Calm);
        SetBabyHeadFollow(true, duration: 2f);
        SetBabySatAndLum(1f, 1f, 1f);
        // SetBabyScale(0.5f);
        Log.StateLog("Baby Appaise Calm State Enter");
        stateIsActive = true;
        LoopBabyAnimation();
    }

    public override void UpdateState()
    {
        Context.TransitionBabyAnimation();
    }
    
    private async void LoopBabyAnimation()
    {
        while (stateIsActive)
        {
            SetBabyAnimation(0f, AnimName.Appaise_Dodo);
            SetBabyAnimation(1f, AnimName.Appaise_Calm);
            await Task.Delay(UnityEngine.Random.Range(5000, 7000));
            if (!stateIsActive) return;
            SetBabyAnimation(0f, AnimName.Appaise_Calm);
            SetBabyAnimation(1f, AnimName.Appaise_Dodo);

            await Task.Delay(UnityEngine.Random.Range(5000, 7000));
            if (!stateIsActive) return;
        }
    }
    

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.AppaiseCalm;
    }

    public override void ExitState()
    {

        stateIsActive = false;
        SetTransitionSpeed(0.3f);
        SetBabyAnimation(0f, AnimName.Appaise_Calm);
        SetBabyAnimation(0f, AnimName.Appaise_Dodo);
        SetBabyHeadFollow(false, duration: 1f);
        Log.StateLog("Baby Appaise Calm State Exit");
    }



}
