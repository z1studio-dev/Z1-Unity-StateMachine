using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


[Serializable]              
public class StateEventFilter<EState> where EState : Enum
{
    public EState  state;
    public UnityEvent onEnter;
    public UnityEvent onExit;
}
public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> statesDictionary = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;
    [SerializeField] private EState _currentState; //for initial state and display state in inspector


    [Header("State-specific events")]
    [SerializeField] private List<StateEventFilter<EState>> _stateEvents = new();

    public EState GetCurrentState => _currentState;

    private bool isTransitioning = false;
    private void Start()
    {
        currentState = statesDictionary[_currentState];
        currentState.EnterState();
    }

    public virtual void OnTransitionOver()
    {
    }

    public virtual void OnTransitionStart()
    {
    }

    public virtual void Update()
    {
        Result<EState> nextStateRes = currentState.GetNextStateifConditionMet();
        if (nextStateRes.stateChanged)
        {
            ApplyNextStateSettings(nextStateRes.nextState);
            OnTransitionStart();
        }
        
        if(isTransitioning)
        {
            isTransitioning = currentState.TransitionToState();
            if (!isTransitioning)
            {
                OnTransitionOver();
                Log.LogMe(GetType().Name, $"Transition to {nextStateRes.nextState} finished!");
            }
        }
        else
        {
            currentState.UpdateState();
        }
    }
    
    public void ApplyNextStateSettings(EState nextStateKey)
    {
        Log.LogMe(GetType().Name, $"Transition from {currentState.GetStateKey()} to {nextStateKey} Start!");
        currentState.ExitState();
        //OnStateExit.Invoke(currentState.GetStateKey());
        foreach (var s in _stateEvents)         // new per-state list
            if (EqualityComparer<EState>.Default.Equals(s.state, currentState.GetStateKey()))
                s.onExit?.Invoke();

        currentState = statesDictionary[nextStateKey];
        currentState.ResetConditions();
        currentState.EnterState();
        foreach (var s in _stateEvents)
        if (EqualityComparer<EState>.Default.Equals(s.state, nextStateKey))
            s.onEnter?.Invoke();

        isTransitioning = currentState.TransitionToState();
        //OnStateEnter.Invoke(nextStateKey);
        _currentState = nextStateKey;
    }

    public void AddState(BaseState<EState> state)
    {
        if (statesDictionary.ContainsKey(state.GetStateKey()))
            throw new ArgumentException($"State '{statesDictionary[state.GetStateKey()]}' is already registered with key '{state.GetStateKey()}'. Cannot register new state '{state}'.");
        
        statesDictionary.Add(state.GetStateKey(), state);
    }

}