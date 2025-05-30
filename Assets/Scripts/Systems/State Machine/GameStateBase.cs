using UnityEngine;

public class GameStateBase : StateBase<GameStates>
{
    public override StateMachine<GameStates> StateMachine { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override GameStates Key { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override bool IsEntered { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void LateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
