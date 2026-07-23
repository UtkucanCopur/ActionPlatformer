using UnityEngine;

public class AttackState : IState
{
    private PlayerController _player;
    

    public AttackState(PlayerController player) => _player = player;

    public void Enter()
    {
        _player.TriggerAttackAnimation();
        _player.attackRequested = false;
    }

    public void Update() 
    {

    }
    
    public void Exit()
    {
        _player.lastAttackTime = Time.time;

    }

}
