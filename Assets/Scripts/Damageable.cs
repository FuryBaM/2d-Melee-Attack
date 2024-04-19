using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float _maxHP;
    public float MaxHP { get { return _maxHP; } }
    private float _currentHP;
    public float CurrentHP { get { return _currentHP; } }
    public delegate void HealthChanged(float value);
    public event HealthChanged OnTakeDamage;
    public event HealthChanged OnHeal;
    private bool _ignoreDamage;
    [SerializeField] private float _ignoreDamageTime = 0.5f;
    private float _currentIgnoreDamageTime = 0;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetMaxHealth();
    }
    private void Update()
    {
        _currentIgnoreDamageTime -= Time.deltaTime;
        if (_currentIgnoreDamageTime < 0)
        {
            _ignoreDamage = false;
        }
    }
    private void SetMaxHealth()
    {
        _currentHP = _maxHP;
    }
    public void TakeDamage(float damage)
    {
        if (_ignoreDamage == true) return;
        damage = Mathf.Abs(damage);
        _currentHP = Mathf.Max(0, _currentHP - damage);
        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
        _ignoreDamage = true;
        _currentIgnoreDamageTime = _ignoreDamageTime;
        OnTakeDamage?.Invoke(damage);
    }
    public void TakeDamage(float damage, Vector2 direction)
    {
        if (_ignoreDamage == true) return;
        damage = Mathf.Abs(damage);
        _currentHP = Mathf.Max(0, _currentHP - damage);
        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
        if (rb.velocity.y == 0)
            rb.AddForce(direction, ForceMode2D.Impulse);
        _ignoreDamage = true;
        _currentIgnoreDamageTime = _ignoreDamageTime;
        OnTakeDamage?.Invoke(damage);
    }
    public void Heal(float heal)
    {
        heal = Mathf.Abs(heal);
        _currentHP = Mathf.Min(_maxHP, _currentHP + heal);
        OnHeal?.Invoke(heal);
    }
}
