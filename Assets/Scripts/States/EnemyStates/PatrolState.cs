using UnityEngine;

public class PatrolState : IState
{
    private EnemyController _enemy;
    private int _waypointIndex;

    public PatrolState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.SetMoveAnimation(true);
    }

    public void Update()
    {
        Transform target = _enemy.Waypoints[_waypointIndex];

        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position,
            target.position, _enemy.Stats.MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(_enemy.transform.position, target.position) < 1f)
        {
            _waypointIndex = (_waypointIndex + 1) % _enemy.Waypoints.Length;
        }

        float direction = target.position.x - _enemy.transform.position.x;
        _enemy.HandleFlip(direction);
    }

    

    public void Exit() { }


}
