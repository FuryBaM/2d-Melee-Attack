using System.Collections.Generic;
using UnityEngine;

public class AIAttackController : MonoBehaviour
{
    [HeaderAttribute("Attack")]
    [SerializeField] private AudioSource _hitAudioSource;
    [SerializeField] private float _damage;
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private LayerMask _enemyLayer;
    private bool _canAttack = true;
    public bool CanAttack { get { return _canAttack; } }
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private AIMovement _movement;

    public void SimulateAttack()
    {
        if (!_canAttack || _movement.HorizontalMove != 0 || !_movement.controller.Grounded) return;
        _animator.SetTrigger("Attack");
    }
    public void SimalateAttackSound()
    {
        _hitAudioSource.Play();
    }
    public void EnableAttack() => _canAttack = true;
    public void DisableAttack() => _canAttack = false;

    public void DamageTarget()
    {
        List<Collider2D> hitColliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(_enemyLayer);
        _hitCollider.OverlapCollider(contactFilter, hitColliders);
        foreach (Collider2D collider in hitColliders)
        {
            Damageable damageable;
            if (collider.TryGetComponent(out damageable))
            {
                Vector2 direction = (damageable.transform.position - transform.position).normalized * 3f;
                damageable.TakeDamage(_damage, direction);
            }
        }
    }
}
