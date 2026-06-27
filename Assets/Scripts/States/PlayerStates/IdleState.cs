using UnityEngine;

public class IdleState : IState
{
    private PlayerController _player;


    public IdleState(PlayerController player) => _player = player;

    public void Enter()
    {

    }

    public void Update()
    {
        if (_player.MoveInput != Vector2.zero)
            _player.StateMachine.ChangeState(_player.MoveState);
    }
    public void Exit() { }
}
