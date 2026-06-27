using UnityEngine;

public class AttackState : IState
{
    private PlayerController _player;
    
    private LayerMask _enemyLayer;

    public AttackState(PlayerController player) => _player = player;

    public void Enter()
    {
        
    }

    public void Update() { }
    public void Exit() { }

}
