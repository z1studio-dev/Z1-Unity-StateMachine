using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BabyInteractionContext 
{
    private Animator _animator;
    private Material _babyMaterial;
    private Rigidbody _rigidbody;
    private List<BabyAnimMeshActivator> _babyAnimMeshActivators;
    private GameObject _babyParentObject;
    private BabyHeadFollowUsController _babyHeadFollowUs;
    private BabyDampedRigController _babyDampedRigController;
    private SaturatorController _babySaturatorController;
    private BabyStateMachine _babyStateMachine;
    
    protected float _transitionSpeed = 1f;
    protected Vector3 _babyScale = Vector3.one;
    //protected Dictionary<string, Color> _colorParamters = new Dictionary<string, Color>();
    protected Dictionary<string, float> _animationParameters = new Dictionary<string, float>();
    protected Dictionary<string, float> _layerParameters = new Dictionary<string, float>();
    

    public BabyInteractionContext(Animator animator, Material babyMaterial, Rigidbody rigidbody, List<BabyAnimMeshActivator> babyAnimMeshActivators, BabyHeadFollowUsController babyHeadFollowUs, BabyDampedRigController babyDampedRigController, SaturatorController babySaturatorController, BabyStateMachine babyStateMachine)
    {
        _animator = animator;
        _babyMaterial = babyMaterial;
        _rigidbody = rigidbody;
        _babyAnimMeshActivators = babyAnimMeshActivators;
        _babyHeadFollowUs = babyHeadFollowUs;
        _babyDampedRigController = babyDampedRigController;
        _babySaturatorController = babySaturatorController;
        _babyStateMachine = babyStateMachine;
    }

    public Animator GetAnimator => _animator; 
    public Material GetBabyMaterial => _babyMaterial;
    public Rigidbody Rigidbody => _rigidbody;
    public float TransitionSpeed => _transitionSpeed;
    public BabyHeadFollowUsController BabyHeadFollowUs => _babyHeadFollowUs;
    public BabyDampedRigController BabyDampedRigController => _babyDampedRigController;
   // public Dictionary<string, Color> ColorParamters => _colorParamters;
    public Dictionary<string, float> AnimationParameters => _animationParameters;
    public Dictionary<string, float> LayerParameters => _layerParameters;
    public Vector3 BabyScale => _babyScale;
    public SaturatorController SaturatorController => _babySaturatorController;

    public void SetTransitionSpeed(float speed){
        _transitionSpeed = speed;
    }
    public void SetBabyTargetScale(Vector3 scale)
    {
        _babyScale = scale;
    }

    public void EnableMoveUpAndDown(bool enable)
    {
        var moveUpDown = _rigidbody.gameObject.GetComponent<MoveUpAndDown>();
        if (moveUpDown != null) {
            moveUpDown.enableMouvement = enable;
        }
        else
        {
            Log.LogError(GetType().Name, "MoveUpAndDown not found in baby parent object");
        }
    }

    public void ArcMoverStart()
    {
        var arcMover = _rigidbody.gameObject.GetComponent<ArcMover>();
        if (arcMover != null)
        {
            arcMover.duration = 4f;
            arcMover.usePhysics = true;
            arcMover.BeginMove();
        }
    }

    public void HandAttractorTrack(bool enable)
    {
        var handAttractor = _rigidbody.gameObject.GetComponent<AttractGameObjectToHand>();
        if (handAttractor != null)
        {
            handAttractor.enable = enable;
        }
    }
    
    public bool TransitionBabyAnimation()
    {
        bool isTransitioning = false;

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
            // if(lerpedValue > 0.5f){
            //     UpdateBabyMesh(animationParameter.Key);
            // }
            //_animator.SetFloat(animationParameter.Key, animationParameter.Value);
            isTransitioning = true;
        }
        return isTransitioning;
    }

    public bool TransitionBabyLayer()
    {
        bool isTransitioning = false;
        foreach (KeyValuePair<string, float> layerParameter in _layerParameters)
        {
            float currentLayerWeightValue = _animator.GetLayerWeight(_animator.GetLayerIndex(layerParameter.Key));
            if (Mathf.Abs(currentLayerWeightValue - layerParameter.Value) < 0.05f)
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex(layerParameter.Key), layerParameter.Value);
                continue;
            }
            float lerpedValue = Mathf.Lerp(currentLayerWeightValue, layerParameter.Value, Time.deltaTime * _transitionSpeed);
            _animator.SetLayerWeight(_animator.GetLayerIndex(layerParameter.Key), lerpedValue);
            isTransitioning = true;
        }
        return isTransitioning;
    }
    
    public void UpdateBabyMesh(){
        foreach (BabyAnimMeshActivator babyAnimMeshActivator in _babyAnimMeshActivators)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in babyAnimMeshActivator.skinnedMeshRenderers)
            {
                if (babyAnimMeshActivator.State == _babyStateMachine.GetCurrentState)
                {
                    skinnedMeshRenderer.enabled = true;
                }
                else
                {
                    skinnedMeshRenderer.enabled = false;
                }
            }
        }
    }

// This method is commented out because it is replaced by the SaturatorController
/*
    public bool TransitionBabyColor(){
        bool isTransitioning = false;
        foreach (KeyValuePair<string, Color> colorParameter in _colorParamters)
        {     
            Color currentColor = _babyMaterial.GetColor(colorParameter.Key);
            if (currentColor == colorParameter.Value)
            {
                continue;
            }
            Color lerpedColor = Color.Lerp(currentColor, colorParameter.Value, Time.deltaTime * _transitionSpeed);
            _babyMaterial.SetColor(colorParameter.Key, lerpedColor);
            isTransitioning = true;
        }
        return isTransitioning;
    }
*/
    public bool TransitionBabyScale(){
        Vector3 currentScale = _rigidbody.transform.localScale;
        if (currentScale == _babyScale)
        {
            return false;
        }
        Vector3 lerpedScale = Vector3.Lerp(currentScale, _babyScale, Time.deltaTime * _transitionSpeed);
        _rigidbody.transform.localScale = lerpedScale;
        return true;
    }

    public void SetBabySaturationAndLuminosirty(float saturation, float luminosity, float? duration = null)
    {
        _babySaturatorController.TransitionSaturationAndLuminosity(saturation, luminosity, duration);
    }
    
}
