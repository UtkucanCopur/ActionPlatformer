using UnityEngine;

public class EnemyAttackState : IState
{
    private EnemyController _enemy;

    public EnemyAttackState(EnemyController enemy) => _enemy = enemy;


    public void Enter()
    {
        _enemy.SetVelocityZero();
        _enemy.TriggerAttackAnimation();

        _enemy.lastAttackTime = Time.time;
    }

    public void Update()
    {

    }

    public void Exit()
    {
        
    }



}
