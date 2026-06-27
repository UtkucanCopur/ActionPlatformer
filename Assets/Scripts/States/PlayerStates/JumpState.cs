using UnityEngine;

public class JumpState : IState
{

    private PlayerController _player;
    
    public JumpState(PlayerController player) => _player = player;


    public void Enter()
    {
        _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocity.x, _player.Stats.JumpForce);
    }

    public void Update()
    {
        _player.Rb.linearVelocity = new Vector2(_player.MoveInput.x * _player.Stats.MoveSpeed, _player.Rb.linearVelocity.y);

        if (_player.IsGrounded() && _player.Rb.linearVelocity.y <= 0.1f)
        {
            _player.StateMachine.ChangeState(_player.IdleState);
        }
    }

    public void Exit() { }

}
