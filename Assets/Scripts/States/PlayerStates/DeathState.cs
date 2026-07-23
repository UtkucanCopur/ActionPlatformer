using UnityEngine;

public class DeathState : IState
{

    private PlayerController _player;

    public DeathState(PlayerController player) => _player = player;

    public void Enter()
    {
        Debug.Log("Player is dead");
        _player.TriggerDeathAnimation();
    }
    

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
