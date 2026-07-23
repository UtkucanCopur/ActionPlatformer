using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, IDamageable
{
    public StateMachine StateMachine {  get; private set; }
    public PlayerStats Stats;


    public PlayerInputActions InputActions;
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }
    public AttackState AttackState { get; private set; }


    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public Vector2 MoveInput { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    private bool _isFacingRight = true;


    [Header("Collision Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [HideInInspector] public float lastAttackTime;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] private float _health;

    [Header("Combo Settings")]
    public int comboStep = 0;
    public bool canCombo = false;
    public bool attackRequested = false;



    private void Awake()
    {
        _health = Stats.MaxHealth;

        Rb = GetComponent<Rigidbody2D>();
        InputActions = new PlayerInputActions();

        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        JumpState = new JumpState(this);
        AttackState = new AttackState(this);

        StateMachine = new StateMachine();

    }

    private void OnEnable() => InputActions.Enable();
    private void OnDisable() => InputActions.Disable();

    void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    void Update()
    {

        if (InputActions.Player.Jump.triggered && IsGrounded())
        {
            StateMachine.ChangeState(JumpState);
        }
        if (InputActions.Player.Attack.triggered)
        {
            if (comboStep < 3)
            {
                comboStep++;
                canCombo = false;

                TriggerAttackAnimation();
            }
            if (canCombo && StateMachine.CurrentState == AttackState)
            { 
                attackRequested = true;
            }
            else if (Time.time >= lastAttackTime + Stats.AttackCooldown && StateMachine.CurrentState != AttackState)
            {
                ResetCombo();
                StateMachine.ChangeState(AttackState);
            }

        }
        Flip();
        
        StateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        MoveInput = InputActions.Player.Move.ReadValue<Vector2>();
        
        if (!IsGrounded()) 
        {
            if (Rb.linearVelocity.y > 0.1f)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
            else if (Rb.linearVelocity.y < -0.1f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        

    }




    public void SetMoveAnimation(bool isMoving) => animator.SetBool("isMoving", isMoving);
    public void SetJumpAnimation(bool isJumping) => animator.SetBool("isJumping", isJumping);
    public void TriggerAttackAnimation()
    {
        if (comboStep == 1) { animator.SetTrigger("Attack"); }
        else if (comboStep == 2) { animator.SetTrigger("Attack2"); }
        else if (comboStep == 3) { animator.SetTrigger("Attack3"); }
    }
    public void TriggerIdleAnimation() => animator.SetTrigger("Idle");
    public void TriggerDeathAnimation() => animator.SetTrigger("Death");

    public void SetVelocityZero()
    {
        Rb.linearVelocity = new Vector2 (0f, Rb.linearVelocity.y);
    }



    public void ResetCombo()
    {
        comboStep = 1;
        canCombo = false;
        attackRequested = false;
    }

    public void EnableCombo()
    {
        canCombo = true;
    }

    public void CheckNextCombo()
    {
        if (attackRequested && comboStep < 3)
        {
            comboStep++;
            canCombo = false;
            attackRequested = false;
            StateMachine.ChangeState(new AttackState(this));
        }
        else
        {
            ResetCombo();
            StateMachine.ChangeState(IdleState);
        }
    }
    
    public void TakeDamage(float amount)
    {
        _health -= amount;
        _health = Mathf.Clamp(_health, 0f, Stats.MaxHealth);

        if (_health <= 0f)
        {
            StateMachine.ChangeState(new DeathState(this));
        }
        Debug.Log(_health);
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }


    public void PerformAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<IDamageable>(out var damageable) && !hitCollider.CompareTag("Player"))
            {
                damageable.TakeDamage(5f);
                Debug.Log("vurdu");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, 3f);
    }


    public void OnAttackFinished()
    {
        StateMachine.ChangeState(new IdleState(this));
    }

    private void Flip()
    {
        if (_isFacingRight && MoveInput.x < 0 || !_isFacingRight && MoveInput.x > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius,
            groundLayer);
    }


}
