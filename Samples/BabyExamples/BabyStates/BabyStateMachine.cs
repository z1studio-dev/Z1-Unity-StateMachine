using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;
using UnityEngine.Animations.Rigging;

[Serializable]
public struct BabyAnimMeshActivator{
    public BabyStateMachine.EBabyState State;
    public List<SkinnedMeshRenderer> skinnedMeshRenderers;
}

public class BabyStateMachine : StateManager<BabyStateMachine.EBabyState>
{
    public enum EBabyState{
        None,
        AppaiseCalm,
        AppaiseDodo,
        AppaisePouce,
        DetressePleure,
        DetresseBoule,
        DetresseFreeze,
        DetresseInerte,
        DetressePetitMouvement,
        DetressePresqueEvanoui,
        Floating,
        CinematicSC2A,
        WakingUp,
        Idle,
        Souffle,
        GoInsideBaleine
    }

    EBabyState[] appaiseState = new[] {EBabyState.AppaiseDodo, EBabyState.AppaisePouce, EBabyState.AppaiseCalm};
    EBabyState[] detresseState = new[] {EBabyState.DetresseFreeze, EBabyState.DetresseInerte, EBabyState.DetresseBoule, EBabyState.DetressePetitMouvement, EBabyState.DetressePleure, EBabyState.DetressePresqueEvanoui};
    
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Material _babyMaterial;
    [SerializeField] private Rigidbody _rigidbody;   
    [SerializeField] private BabyHeadFollowUsController _babyHeadFollowUsController;
    [SerializeField] private SaturatorController _babySaturatorController;
    [SerializeField] private BabyDampedRigController _babyDampedRigController;
    [SerializeField] private List<BabyAnimMeshActivator> _babyAnimMeshActivators;

    private BabyInteractionContext _context;

    void Awake()
    {
        ValidatePublicVariable();
        _context = new BabyInteractionContext(_animator, _babyMaterial, _rigidbody, _babyAnimMeshActivators, _babyHeadFollowUsController, _babyDampedRigController, _babySaturatorController, this);
        InitializeStates();
    }

    private void ValidatePublicVariable()
    {
        Assert.IsNotNull(_animator, "Animator is null");
        Assert.IsNotNull(_babyMaterial, "Baby Material is null");
        Assert.IsNotNull(_rigidbody, "Rigidbody is null");
        Assert.IsNotNull(_babyAnimMeshActivators, "Baby Anim Mesh Activators is null");
        Assert.IsNotNull(_babyHeadFollowUsController, "Baby Head Follow Us Controller is null");     
        Assert.IsNotNull(_babySaturatorController, "Baby Saturator Controller is null");   
    }

    private void InitializeStates()
    {
        AddState(new BabyStateNone(_context).
            AddCondition(new ConditionAlwaysFalse<EBabyState>(EBabyState.None))
        );

        AddState(new BabyStateAnimaticSC2A(_context).
            AddCondition(new ConditionAlwaysFalse<EBabyState>(EBabyState.CinematicSC2A))
        );

        AddState(new BabyFloatingState(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.WakingUp, _context.Rigidbody, 0.5f, false))
        );

        AddState(new BabyStateWakingUP(_context).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.DetresseFreeze, HandsSelector.Dual, new Vector2(1.5f, 10f), 0.1f)).
            AddCondition(new ConditionAnd<EBabyState>(EBabyState.AppaiseCalm,
            new ConditionHandsVeclocity<EBabyState>(EBabyState.None, HandsSelector.Dual, new Vector2(0.00f, 1.5f), 15f),
            new ConditionDistance<EBabyState>(EBabyState.None, _context.Rigidbody, 0.4f, false))).
            AddCondition(new ConditionTouch<EBabyState>(EBabyState.AppaiseCalm, 2f))
        );

        AddState(new BabyDetresseFreezeState(_context).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.WakingUp, HandsSelector.Dual, new Vector2(0.0f, 1.5f), 2f)).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.DetresseInerte, HandsSelector.Dual, new Vector2(1.5f, 10f), 2f))
        );

        AddState(new BabyDetresseInerteState(_context).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.WakingUp, HandsSelector.Dual, new Vector2(0.0f, 1.5f), 2f))
        );

        AddState(new BabyAppaiseCalmState(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.WakingUp, _context.Rigidbody, 0.5f, true)).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.DetresseFreeze, HandsSelector.Dual, new Vector2(1.5f, 10f), 0f)).
            AddCondition(new ConditionHandsVeclocity<EBabyState>(EBabyState.AppaiseDodo, HandsSelector.Dual, new Vector2(0.0f, 1.5f), 6f))
        );

/* UNUSED
        AddState(new BabyAppaisePouceState(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.WakingUp, _context.Rigidbody, 0.5f, true)).
            AddCondition(new ConditionHandsVeclocity(EBabyState.AppaiseDodo, HandsSelector.Dual, new Vector2(0.0f, 1.5f), 4f))
        );
 */       
        AddState(new BabyAppaiseDodoState(_context));
        
        AddState(new BabySouffleState(_context).
            AddCondition(new ConditionSouffle<EBabyState>(EBabyState.GoInsideBaleine, 30f)));
        
        AddState(new BabyGoInsideBaleineState(_context));
        
        /*
        AddState(new BabyDetressePleureState(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.AppaiseCalm, _context.Rigidbody, 0.5f, false)).   
            AddCondition(new ConditionRndStateAfterRandomSeconds<EBabyState>(3f, 9f, detresseState))
        );
        
        AddState(new BabyDetressePLState(_context).
            //AddCondition(new ConditionIsTrueOrFalse(EBabyState.None, false))
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.AppaiseCalm, _context.Rigidbody, 0.5f, false)).
            AddCondition(new ConditionRndStateAfterRandomSeconds<EBabyState>(3f, 9f, detresseState))
            );

        AddState(new BabyDetressePetitMouvement(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.AppaiseCalm, _context.Rigidbody, 0.5f, false)).
            AddCondition(new ConditionRndStateAfterRandomSeconds<EBabyState>(3f, 9f, detresseState))
            );
        
        AddState(new BabyDetresseFaintingState(_context).
            AddCondition(new ConditionDistance<EBabyState>(EBabyState.AppaiseCalm, _context.Rigidbody, 0.5f, false)).
            AddCondition(new ConditionRndStateAfterRandomSeconds<EBabyState>(3f, 9f, detresseState))
            );
        
        AddState(new BabyIdleState(_context).
            AddCondition(new ConditionAlwaysFalse(EBabyState.None))
        );

        */
    }

    public override void OnTransitionOver()
    {
        _context.UpdateBabyMesh();
    }

    public EBabyState RandomAppaiseState()
    {
        var random = new Random();
        EBabyState nextState = appaiseState[random.Next(appaiseState.Length)];
        return nextState;
    }
    
    public EBabyState RandomDetresseState()
    {
        var random = new Random();
        EBabyState nextState = detresseState[random.Next(appaiseState.Length)];
        return nextState;
    }
    public override void Update()
    {
        base.Update();
    }
}