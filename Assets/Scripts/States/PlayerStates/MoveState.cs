using UnityEngine;


public class MoveState : IState
{
    private PlayerController _player;

    public MoveState(PlayerController player) => _player = player;



    public void Enter()
    {
        _player.SetMoveAnimation(true);
    }

    public void Update()
    {
     
        float speed = _player.Stats.MoveSpeed;
        _player.Rb.linearVelocity = new Vector2(_player.MoveInput.x * speed,_player.Rb.linearVelocity.y);

        if (_player.MoveInput == Vector2.zero)
            _player.StateMachine.ChangeState(_player.IdleState);
    }

    public void Exit() 
    { 
        _player.Rb.linearVelocity = Vector2.zero;
        _player.SetMoveAnimation(false);
    }


}
