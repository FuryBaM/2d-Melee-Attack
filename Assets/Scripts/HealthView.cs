using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(Damageable))]
public class HealthView : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Animator _animator;
    private Damageable damageable;
    private void Start()
    {
        damageable = GetComponent<Damageable>();
        damageable.OnTakeDamage += OnHeal;
        damageable.OnTakeDamage += OnTakeDamage;
    }
    private void OnHeal(float damage)
    {
        _healthBar.fillAmount = damageable.CurrentHP / damageable.MaxHP;
    }

    private void OnTakeDamage(float damage)
    {
        _healthBar.fillAmount = damageable.CurrentHP / damageable.MaxHP;
        _animator.SetTrigger("Hurt");
    }
}
