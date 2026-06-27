using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public StateMachine StateMachine {  get; private set; }
    public Transform[] Waypoints;
    public EnemyStats Stats;

    private void Awake()
    {
        StateMachine = new StateMachine();
        StateMachine.Initialize(new PatrolState(this));
    }

    private void Update()
    {
        if (StateMachine != null && StateMachine.CurrentState != null)
        {
            StateMachine.CurrentState.Update();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(10f);
            
        }
    }

    

}
