using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator animator;


    public Transform PlayerTransform;
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

    public void SetMoveAnimation(bool isMoving) => animator.SetBool("isMoving", isMoving);
    public void TriggerAttackAnimation() => animator.SetTrigger("Attack");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player is founded");
            StateMachine.ChangeState(new ChaseState(this));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player is flew");
            StateMachine.ChangeState(new PatrolState(this));
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(10f);
            
        }
    }

    public void HandleFlip(float directionX)
    {
        // directionX > 0 ise sağa (1), < 0 ise sola (-1) bakmalı
        if (directionX != 0)
        {
            // 0.1f tolerans ile flip yapıyoruz ki gereksiz tetiklenmesin
            Vector3 newScale = transform.localScale;
            newScale.x = -Mathf.Sign(directionX) * Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
    }



}
