using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _groundLayer;
    private Transform _target;
    [SerializeField] private float _targetDetectionRange = 10f;
    [SerializeField] private float _stopDistance;
    private float _leftBorder;
    private float _rightBorder;
    [SerializeField] private float _maxPatrolDistance = 10f;
    private bool _patrolRight = true;
    private AIMovement movement;
    private AIAttackController attackController;
    private CharacterController2D characterController;
    private void Start()
    {
        movement = GetComponent<AIMovement>();
        attackController = GetComponent<AIAttackController>();
        characterController = GetComponent<CharacterController2D>();
        _leftBorder = _rightBorder = transform.position.x;
    }
    private void Update()
    {
        if (!_target)
        {
            if (Mathf.Min(Mathf.Abs(transform.position.x - _leftBorder), Mathf.Abs(transform.position.x - _rightBorder)) > 1f)
            {
                _leftBorder = _rightBorder = transform.position.x;
            }
            if (Mathf.Abs(_leftBorder - _rightBorder) <= _maxPatrolDistance)
            {
                ExpandPatrolArea();
            }
            else
            {
                Patrol();
            }
            DetectTarget();
        }
        else
        {
            if (Vector2.Distance(transform.position, _target.position) < _targetDetectionRange)
                MoveToTarget();
            else
                _target = null;
        }
    }

    private void Patrol()
    {
        print("Patroling");
        if (transform.position.x < _leftBorder)
        {
            _patrolRight = true;
        }
        else if (transform.position.x > _rightBorder)
        {
            _patrolRight = false;
        }
        if (_patrolRight)
        {
            movement.Move(Vector2.right);
        }
        else
        {
            movement.Move(Vector2.left);
        }
    }

    private void ExpandPatrolArea()
    {
        print("Exploring");
        Vector2 rayOriginGround = _patrolRight ? new Vector2(transform.position.x+0.1f, transform.position.y) : new Vector2(transform.position.x-0.1f, transform.position.y);
        Vector2 rayOriginObstacle = _patrolRight ? new Vector2(transform.position.x, transform.position.y+0.5f) : new Vector2(transform.position.x, transform.position.y+0.5f);
        Vector2 rayDirection = _patrolRight ? Vector2.right : Vector2.left;

        RaycastHit2D hitGround = Physics2D.Raycast(rayOriginGround, rayDirection, 1f, _groundLayer);
        RaycastHit2D hitObstacle = Physics2D.Raycast(rayOriginObstacle, rayDirection, 1f, _groundLayer);

        if (hitGround.collider == null || hitObstacle.collider != null)
        {
            _patrolRight = !_patrolRight;
        }
        else
        {
            Vector2 patrolDirection = _patrolRight ? Vector2.right : Vector2.left;
            movement.Move(patrolDirection);
            
            if (_patrolRight)
            {
                _rightBorder = transform.position.x;
            }
            else
            {
                _leftBorder = transform.position.x;
            }
        }
    }

    private void DetectTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _targetDetectionRange, _targetLayer);

        if (targets.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Collider2D targetCollider in targets)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetCollider.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    _target = targetCollider.transform;
                }
            }
        }
        else
        {
            _target = null;
        }
    }

    private bool _hasJumped = false;

    private void MoveToTarget()
    {
        if (!_target) return;

        Vector2 rayOriginGround = _patrolRight ? new Vector2(transform.position.x + 0.1f, transform.position.y) : new Vector2(transform.position.x - 0.1f, transform.position.y);
        Vector2 direction = (_target.position - transform.position).normalized;
        Vector2 rayOriginObstacle = _patrolRight ? new Vector2(transform.position.x, transform.position.y + 0.5f) : new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D hitGround = Physics2D.Raycast(rayOriginGround, direction, 1f, _groundLayer);
        RaycastHit2D hitObstacle = Physics2D.Raycast(rayOriginObstacle, _patrolRight ? Vector2.right : Vector2.left, 1f, _groundLayer);

        if (Vector2.Distance(_target.position, transform.position) < _stopDistance)
        {
            if (characterController.FacingRight && direction.x < 0)
            {
                characterController.Flip();
            }
            direction = Vector2.zero;
            attackController.SimulateAttack();
        }

        if (hitObstacle.collider != null && !_hasJumped)
        {
            movement.Jump();
            _hasJumped = true;
        }
        else if (hitObstacle.collider == null)
        {
            _hasJumped = false;
        }
        else
        {
            movement.Move(direction);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(_leftBorder, transform.position.y, transform.position.z), new Vector3(_rightBorder, transform.position.y, transform.position.z));
        
        Vector2 rayDirection = _patrolRight ? Vector2.right : Vector2.left;

        Vector2 rayOriginGround = _patrolRight ? new Vector2(transform.position.x+0.1f, transform.position.y) : new Vector2(transform.position.x-0.1f, transform.position.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rayOriginGround, rayOriginGround + rayDirection);

        Vector2 rayOriginObstacle = _patrolRight ? new Vector2(transform.position.x, transform.position.y+0.5f) : new Vector2(transform.position.x, transform.position.y+0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOriginObstacle, rayOriginObstacle + rayDirection);

        Gizmos.DrawWireSphere(transform.position, _targetDetectionRange);
    }
}
