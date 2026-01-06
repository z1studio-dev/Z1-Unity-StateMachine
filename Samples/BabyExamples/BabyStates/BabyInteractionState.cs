using System;
using UnityEngine;

public abstract class BabyInteractionState : BaseState<BabyStateMachine.EBabyState>
{
    protected BabyInteractionContext Context;
    
    protected struct AnimName
    {
        public const string Appaise_Calm = "Appaise_Calme";
        public const string Appaise_Dodo = "Appaise_Dodo";
        public const string Appaise_Pouce = "Appaise_Pouce";
        public const string Detresse_Pleure = "Detresse_Pleure";
        public const string Detresse_Boule = "Detresse_Boule";
        public const string Detresse_Freeze = "Detresse_Freeze";
        public const string Detresse_Inerte = "Detresse_Inerte";
        public const string Detresse_PetitMouvement = "Detresse_PetitMouvement";
        public const string Detresse_PresqueEvanoui = "Detresse_PresqueEvanoui";
        public const string BabyIdle = "Baby_Idle";
        //public const string Floating = "Floating";
    }

    protected struct LayerName
    {
        public const string FloatingLayer = "FloatingMouvement";
    }
    
    public BabyInteractionState(BabyInteractionContext context)
    {
        Context = context;
    }

    protected void SetBabyAnimation(float targetValue, string parameterName)
    {

        if (Context.AnimationParameters.ContainsKey(parameterName))
        {
            Context.AnimationParameters[parameterName] = targetValue;
        }
        else
        {
            Context.AnimationParameters.Add(parameterName, targetValue);
        }
    }
    
    protected void DebugBabyAnimation()
    {
        foreach (var parameter in Context.AnimationParameters)
        {
            Debug.Log($"Animation Parameter: {parameter.Key} : {parameter.Value}");
        }
    }

    protected void SetBabyLayerWeight(float targetWeight, string layerName)
    {
        if (Context.LayerParameters.ContainsKey(layerName))
        {
            Context.LayerParameters[layerName] = targetWeight;
        }
        else
        {
            Context.LayerParameters.Add(layerName, targetWeight);
        }
    }

    protected void SetBabyHeadFollow(bool enable, float? weight = null, float? duration = null)
    {
        Context.BabyHeadFollowUs.SetFollowState(enable, weight, duration);
    }

    protected void SetBabyDampedRig(float weight, float? duration = null)
    {
        Context.BabyDampedRigController.SetWeight(weight, duration);
    }
    
    /*
    protected void SetBabyColor(Color color, String colorParameter)
    {
        if (Context.ColorParamters.ContainsKey(colorParameter))
        {
            Context.ColorParamters[colorParameter] = color;
        }
        else
        {
            Context.ColorParamters.Add(colorParameter, color);
        }
    }
    */

    protected void SetTransitionSpeed(float speed)
    {
        Context.SetTransitionSpeed(speed);
    }

    protected void DisplayToolTip(bool enable, string toolTip)
    {
        if (enable)
        {
            TooltipManager.Instance.ShowTooltip(toolTip);
        }
        else
        {
            TooltipManager.Instance.HideTooltip(toolTip);
        }
    }

    protected void SetBabyScale(float scale)
    {
        Context.SetBabyTargetScale(new Vector3(scale, scale, scale));
    }

    protected void SetBabyUpDownMouvement(bool enable)
    {
        Context.EnableMoveUpAndDown(enable);
    }

    protected void SetBabySatAndLum(float saturation, float luminosity, float? duration = null)
    {
        Context.SetBabySaturationAndLuminosirty(saturation, luminosity, duration);
    }
    
    //things to do when transitioning to and from a state
    public override bool TransitionToState(){
        return  Context.TransitionBabyAnimation() |
               // Context.TransitionBabyColor()     |
                Context.TransitionBabyScale()     |
                Context.TransitionBabyLayer();
    }

    //things to do during a state
    public override void UpdateState()
    {
        //nothing yet
    }

    public BabyInteractionContext GetContext()
    {
        return Context;
    }
}
