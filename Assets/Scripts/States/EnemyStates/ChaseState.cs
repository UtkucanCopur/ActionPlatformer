using UnityEngine;

public class ChaseState : IState
{
    private EnemyController _enemy;


    public ChaseState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.SetMoveAnimation(true);
    }

    public void Update()
    {
        FollowPlayer();
        FlipDirection();
    }

    public void Exit() { }

    private void FlipDirection()
    {
        float direction = _enemy.PlayerTransform.position.x - _enemy.transform.position.x;
        _enemy.HandleFlip(direction);
    }



    private void FollowPlayer()
    {
        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position,
            _enemy.PlayerTransform.position,
            _enemy.Stats.MoveSpeed * Time.deltaTime);
    }

}
