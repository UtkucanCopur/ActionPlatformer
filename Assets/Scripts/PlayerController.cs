using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
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

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;



    private void Awake()
    {
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
            StateMachine.ChangeState(AttackState);
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
    public void TriggerAttackAnimation() => animator.SetTrigger("Attack");
    public void TriggerIdleAnimation() => animator.SetTrigger("Idle");

    public void SetVelocityZero()
    {
        Rb.linearVelocity = new Vector2 (0f, Rb.linearVelocity.y);
    }

    


    public void PerformAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(5f);
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
