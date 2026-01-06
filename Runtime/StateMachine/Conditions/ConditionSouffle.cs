using System;
using UnityEngine;
public class ConditionSouffle<EState> : StateConditions<EState> where EState : Enum
{
    private float _threshold;

    public ConditionSouffle(EState nextState, float threshold) : base(nextState)
    {
        _threshold = threshold;
        _nextState = nextState;
    }

    public override bool CheckCondition()
    {
        if(AudioLoudnessDetection.instance.loudness > _threshold)
        {
            Debug.Log("ConditionSouffle Detected at: " + AudioLoudnessDetection.instance.loudness);
            return true;
        }else
        {
            return false;
        }
    }
}
