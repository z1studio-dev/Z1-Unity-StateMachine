using UnityEngine;
using System.Collections.Generic;

public class WhaleInteractionContext 
{
    private Animator _animator;
    protected float _transitionSpeed = 1f;
    protected Dictionary<string, float> _animationParameters = new Dictionary<string, float>();

    public WhaleInteractionContext(Animator animator)
    {
        _animator = animator;
    }

    public Animator GetAnimator => _animator; 
    public float TransitionSpeed => _transitionSpeed;
    public void SetTransitionSpeed(float speed){
        _transitionSpeed = speed;
    }

    public Dictionary<string, float> AnimationParameters => _animationParameters;

    public void UpdateWhaleAnimation(){
        foreach (KeyValuePair<string, float> animationParameter in _animationParameters)
        {
            float currentAnimationValue = _animator.GetFloat(animationParameter.Key);
            if (Mathf.Abs(currentAnimationValue - animationParameter.Value) < 0.05f)
            {
                _animator.SetFloat(animationParameter.Key, animationParameter.Value);
                continue;
            }
            float lerpedValue = Mathf.Lerp(currentAnimationValue, animationParameter.Value, Time.deltaTime * _transitionSpeed);
            _animator.SetFloat(animationParameter.Key, lerpedValue);
            //_animator.SetFloat(animationParameter.Key, animationParameter.Value);
        }
    }

}
