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
        if (_enemy.PlayerTransform != null)
        {
            CheckAttackRange();
        }
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

    private void CheckAttackRange()
    {
        float distance = Vector2.Distance(_enemy.transform.position, _enemy.PlayerTransform.position);

        if (distance <= _enemy.attackRange)
        {
            _enemy.SetVelocityZero();
            _enemy.SetMoveAnimation(false);

            if (Time.time >= _enemy.lastAttackTime + _enemy.attackCooldown)
                _enemy.StateMachine.ChangeState(new EnemyAttackState(_enemy));
        } else
        {
            FollowPlayer();
            FlipDirection();
            _enemy.SetMoveAnimation(true);
        }
            
    }


}
