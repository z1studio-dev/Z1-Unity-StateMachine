using UnityEngine;
using UnityEngine.Assertions;

public class WhaleStateMachine : StateManager<WhaleStateMachine.EWhaleState>
{
    public enum EWhaleState
    {
        None,
        Nage,
        Flip
    }
    [SerializeField] private Animator _animator;
    //[SerializeField] private Material _babyMaterial;
    //[SerializeField] private Rigidbody _rigidbody;    

    private WhaleInteractionContext _context;

    void Awake()
    {
        ValidatePublicVariable();
        _context = new WhaleInteractionContext(_animator );
        InitializeStates();
    }

    private void ValidatePublicVariable()
    {
        Assert.IsNotNull(_animator, "Animator is null");
        //Assert.IsNotNull(_babyMaterial, "Baby Material is null");
        //Assert.IsNotNull(_rigidbody, "Rigidbody is null");
    }

    private void InitializeStates()
    {
        AddState(new WhaleNageState(_context).
            AddCondition(new ConditionAfterSeconds<EWhaleState>(EWhaleState.Flip, 20f))
            );

        AddState(new WhaleFlipState(_context).
            AddCondition(new ConditionAfterSeconds<EWhaleState>(EWhaleState.Nage, 20f))
            );
    }
    public override void Update()
    {
        base.Update();
    }
}
