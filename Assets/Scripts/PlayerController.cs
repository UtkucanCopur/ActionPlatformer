using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine StateMachine {  get; private set; }
    public PlayerStats Stats;


    public PlayerInputActions InputActions;
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }


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
        MoveInput = InputActions.Player.Move.ReadValue<Vector2>();

        if (InputActions.Player.Jump.triggered && IsGrounded())
        {
            StateMachine.ChangeState(JumpState);
        }
        Flip();

        StateMachine.CurrentState.Update();
    }

    public void SetMoveAnimation(bool isMoving) => animator.SetBool("isMoving", isMoving);

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
