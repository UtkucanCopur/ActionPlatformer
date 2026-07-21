using UnityEngine;

public class EnemyDeathState : IState
{
    private EnemyController _enemy;


    public EnemyDeathState(EnemyController enemy)
    {
        _enemy = enemy;
    }



    public void Enter()
    {
        _enemy.TriggerDeathAnimation();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        
    }




}
