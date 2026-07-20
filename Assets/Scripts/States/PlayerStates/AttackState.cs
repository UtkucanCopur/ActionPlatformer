using UnityEngine;

public class AttackState : IState
{
    private PlayerController _player;
    

    public AttackState(PlayerController player) => _player = player;

    public void Enter()
    {
        _player.SetVelocityZero();
        _player.TriggerAttackAnimation();
        _player.PerformAttack();
    }

    public void Update() 
    {

    }
    public void Exit() { }

}
