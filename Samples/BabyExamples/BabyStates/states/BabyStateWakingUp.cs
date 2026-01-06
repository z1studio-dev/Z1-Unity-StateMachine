using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BabyStateWakingUP : BabyInteractionState
{
    private float _colorFlickerTimer = 0f;
    private readonly float _flickerInterval = 2f;
    private bool stateIsActive = false;
    public BabyStateWakingUP(BabyInteractionContext context) : base(context)
    {
    }

    public override void EnterState()
    {
        conditions?.ForEach(c => c.ConditionEnter());

        SetTransitionSpeed(.4f);
        //SetBabyAnimation(1f, AnimName.Detresse_PresqueEvanoui);
        SetBabyHeadFollow(true, 0.4f, 2f);
        SetBabySatAndLum(0.4f, 0.6f, 2f);
        stateIsActive = true;
        Log.StateLog("Baby WakingUp State Enter");
        LoopBabyAnimation();

    }

    private async void LoopBabyAnimation()
    {
        while (stateIsActive)
        {
            
            SetBabyAnimation(0f, AnimName.Detresse_Freeze);
            SetBabyAnimation(1f, AnimName.Detresse_PresqueEvanoui);
            await Task.Delay(UnityEngine.Random.Range(5000, 7000));
            if (!stateIsActive) return;
            
            SetBabyAnimation(0f, AnimName.Detresse_PresqueEvanoui);
            SetBabyAnimation(1f, AnimName.Detresse_Pleure);

            await Task.Delay(UnityEngine.Random.Range(5000, 7000));
            if (!stateIsActive) return;
            
            SetBabyAnimation(0f, AnimName.Detresse_Pleure);
            SetBabyAnimation(1f, AnimName.Detresse_Freeze);

            await Task.Delay(UnityEngine.Random.Range(5000, 7000));
            if (!stateIsActive) return;
        }
    }
    
    public override void UpdateState()
    {
       // SetBabyAnimation(1f, AnimName.Detresse_PresqueEvanoui);

        _colorFlickerTimer += Time.deltaTime;

        if (_colorFlickerTimer >= _flickerInterval)
        {
            SetBabySatAndLum(UnityEngine.Random.Range(0.0f, 0.6f), UnityEngine.Random.Range(0.5f, 0.9f), 2f);
            _colorFlickerTimer -= _flickerInterval;
        }
    }

    public override void ExitState()
    {
        conditions?.ForEach(c => c.ConditionExit());

        stateIsActive = false;
        SetTransitionSpeed(2f);
        SetBabyAnimation(0f, AnimName.Detresse_PresqueEvanoui);
        SetBabyAnimation(0f, AnimName.Detresse_Freeze);
        SetBabyAnimation(0f, AnimName.Detresse_Pleure);
        Log.StateLog("Baby WakingUp State Exit");
    }

    public override BabyStateMachine.EBabyState GetStateKey()
    {
        return BabyStateMachine.EBabyState.WakingUp;
    }
}
