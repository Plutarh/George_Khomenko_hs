using System;
using UnityEngine;

public class Pawn : MonoBehaviour, IDamagable
{
    public float Health => _health;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _health;
    public bool IsDead => _isDead;

    public Action OnTakedDamage;
    public Action OnDeath;

    private bool _isDead;


    public virtual void Awake()
    {
        _health = _maxHealth;
    }

    public float GetHealth01()
    {
        if (_health == 0 || _maxHealth == 0)
            return 0;

        float health01 = Mathf.Clamp01(_health / _maxHealth);
        return health01;
    }

    public virtual void TakeDamage(float dmg)
    {
        if (dmg == 0) return;

        _health -= dmg;

        OnTakedDamage?.Invoke();

        if (_health <= 0)
        {
            _health = 0;
            Death();
        }
    }

    public virtual void Death()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }
}
