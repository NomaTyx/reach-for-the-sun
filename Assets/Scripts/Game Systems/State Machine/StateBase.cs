using System;
using UnityEngine;

public abstract class StateBase<EState> where EState : Enum
{
    public abstract StateMachine<EState> StateMachine { get; protected set; }
    public abstract EState Key { get; protected set; }
    public abstract bool IsEntered { get; protected set; }

    //public abstract void Init(StateMachine<EState> machine, params object[] args);

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();
    public abstract void Reset();
    public abstract void Exit();
    public abstract void OnDestroy();
}