using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D Rb;


    public Transform PlayerTransform;
    public StateMachine StateMachine {  get; private set; }
    public Transform[] Waypoints;
    



    [Header("Attack Settings")]
    public float attackRange = 3f;
    public float attackCooldown = 1.5f;
    public Transform attackPosition;
    [HideInInspector] public float lastAttackTime;


    [Header("Stats")]
    private float _health;
    public EnemyStats Stats;

    [Header("UI")]
    public Transform healthBar;
    private float originalBarScaleX;

    private void Awake()
    {
        StateMachine = new StateMachine();
        StateMachine.Initialize(new PatrolState(this));
        SetStartingStats();
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
    public void TriggerDeathAnimation() => animator.SetTrigger("Death");
    public void OnAttackFinished() => StateMachine.ChangeState(new ChaseState(this));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StateMachine.ChangeState(new ChaseState(this));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

    public void SetVelocityZero()
    {
        Rb.linearVelocity = Vector3.zero;
    }

    private void SetStartingStats()
    {
        _health = Stats.MaxHealth;
        if (healthBar != null)
        {
            originalBarScaleX = healthBar.localScale.x;
        }
    }
    
    public void PerformAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<IDamageable>(out var damageable) && !hitCollider.CompareTag("Enemy"))
            {
                damageable.TakeDamage(25f);
            }
        }
    }
    

    public void TakeDamage(float damageAmount)
    {
        _health -= damageAmount;
        _health = Mathf.Clamp(_health, 0, Stats.MaxHealth);
        UpdateHealthBar();
        

        if (_health <= 0) StateMachine.ChangeState(new EnemyDeathState(this));
        Debug.Log(_health);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercent = _health / Stats.MaxHealth;

            Vector3 newScale = healthBar.localScale;
            newScale.x = originalBarScaleX * healthPercent;
            healthBar.localScale = newScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    public void Die()
    {
        Destroy(this.gameObject);
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
