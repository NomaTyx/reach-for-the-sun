using System;

public class GameStateMachine : StateMachine<GameStates>
{
    public event Action<GameStates> GameStateChanged;
}
