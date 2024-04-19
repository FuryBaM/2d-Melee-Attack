using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private float _horizontalMove = 0f;
    public float HorizontalMove {get {return _horizontalMove; } }
    private bool _isJumping;
    [Space]
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }
    private void Update() 
    {
        _animator.SetBool("Fall", !controller.Grounded);
        _animator.SetBool("Run", _horizontalMove!=0 && controller.Grounded);
    }
    public void Jump()
    {
        _isJumping = true && controller.Grounded;
        if (_isJumping)
        {
            _animator.SetTrigger("Jump");
        }
    }
    public void Move(Vector2 direction)
    {
        _horizontalMove = direction.x > 0 ? Mathf.Ceil(direction.x) : Mathf.FloorToInt(direction.x);
    }
    private void FixedUpdate()
    {
        controller.Move(_horizontalMove, false, _isJumping);
        _isJumping = false;
    }
}
