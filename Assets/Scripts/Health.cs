using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;


    private void Awake()
    {
        _currentHealth = maxHealth;        
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(_currentHealth / maxHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
