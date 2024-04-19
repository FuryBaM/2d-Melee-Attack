using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private AudioSource _hitAudioSource;
    [SerializeField] private float _damage;
    [SerializeField] private float _secondaryDamage;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private Collider2D _hitCollider2;
    private bool _isAttacking = false;
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private MovementController _movement;

    private void Update() 
    {
        if (Input.GetButtonDown("Fire1") && _movement.HorizontalMove == 0 && _movement.controller.Grounded)
        {
            SimulateAttack();
        }
        if (Input.GetButtonDown("Fire2") && _movement.HorizontalMove == 0 && _movement.controller.Grounded)
        {
            SimulateSecondaryAttack();
        }
    }

    public void SimulateAttack()
    {   
        if (_isAttacking) return;
        _animator.SetTrigger("Attack1");
    }
    public void SimulateSecondaryAttack()
    {
        if (_isAttacking) return;
        _animator.SetTrigger("Attack2");
    }
    public void SimalateAttackSound()
    {
        _hitAudioSource.Play();
    }

    public void EnableIsAttacking() => _isAttacking = true;
    public void DisableIsAttacking() => _isAttacking = false;

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
            if (collider.TryGetComponent(out damageable) && collider.gameObject != gameObject)
            {
                Vector2 direction = (damageable.transform.position - transform.position).normalized*5f;
                damageable.TakeDamage(_damage, direction);
            }
        }
    }
    public void SecondaryDamageTarget()
    {
        List<Collider2D> hitColliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(_enemyLayer);
        _hitCollider2.OverlapCollider(contactFilter, hitColliders);
        foreach (Collider2D collider in hitColliders)
        {
            Damageable damageable;
            if (collider.TryGetComponent(out damageable))
            {
                Vector2 direction = (damageable.transform.position - transform.position).normalized;
                damageable.TakeDamage(_secondaryDamage, direction);
            }
        }
    }
}
