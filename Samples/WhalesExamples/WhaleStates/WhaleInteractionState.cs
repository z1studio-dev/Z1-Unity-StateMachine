using UnityEngine;

public abstract class WhaleInteractionState : BaseState<WhaleStateMachine.EWhaleState>
{
    protected WhaleInteractionContext Context;
    public WhaleInteractionState(WhaleInteractionContext context)
    {
        Context = context;
    }

    protected void SetWhaleAnimation(float targetValue, string parameterName)
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
    public WhaleInteractionContext GetContext()
    {
        return Context;
    }

    protected void UpdateWhale(){
        Context.UpdateWhaleAnimation();
    }

    public override void UpdateState()
    {
        
    }

    public override bool TransitionToState()
    {
        return false;
    }
}
