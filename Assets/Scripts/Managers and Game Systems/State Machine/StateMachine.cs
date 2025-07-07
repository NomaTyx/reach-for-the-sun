//credit to Apoorv Taneja for coming up with this state management system. thank you for saving me hours of time.
//didnt stop me from putting hours into it anyway tho lollllllllllll
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> where EState : Enum
{
    public Dictionary<EState, StateBase<EState>> States { get; protected set; }

    public StateBase<EState> PreviousState { get; protected set; }
    public StateBase<EState> CurrentState { get; protected set; }
    public StateBase<EState> NextState { get; protected set; }

    // if a non-static event is needed, add one in the inherited state machine
    public static event Action<EState> StateChanged;

    /// <summary>
    /// Accepts a List<StateBase<Estate>> states to populate its dictionary
    /// </summary>
    /// <param name="args"></param>
    public virtual void InitStates(List<StateBase<EState>> states)
    {
        foreach (var state in states)
        {
            States.Add(state.Key, state);
        }
    }

    public virtual void Start() { }

    public virtual void ChangeState(EState Key)
    {
        if (States[Key] == null)
        {
            Debug.Log($"State machine does not contain a state for {Key} key!");
            return;
        }

        // do nothing if the 'new' state is the same
        if (States[Key] == CurrentState)
            return;

        PreviousState = CurrentState;
        NextState = States[Key];

        CurrentState.Exit();
        CurrentState = NextState;
        CurrentState.Enter();

        // Trigger state changed static event
        StateChanged?.Invoke(Key);
    }

    public virtual void Update()
    {
        CurrentState.Update();
    }

    public virtual void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }

    public virtual void LateUpdate()
    {
        CurrentState.LateUpdate();
    }

    public virtual void OnDestroy()
    {
        CurrentState.OnDestroy();
    }
}