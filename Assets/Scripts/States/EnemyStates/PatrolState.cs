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

    }

    public void Update()
    {
        Transform target = _enemy.Waypoints[_waypointIndex];

        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position,
            target.position, _enemy.Stats.MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(_enemy.transform.position, target.position) < 0.1f)
        {
            _waypointIndex = (_waypointIndex + 1) % _enemy.Waypoints.Length;
        }
    }

    public void Exit() { }


}
