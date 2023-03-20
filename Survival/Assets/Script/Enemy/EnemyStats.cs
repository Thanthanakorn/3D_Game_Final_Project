using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Rigidbody _rigidbody;
    public HealthBar healthBar;
    private EnemyAnimatorManager _animatorHandler;

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren<EnemyAnimatorManager>();
        _rigidbody = GetComponent<Rigidbody>();
        isDead = false;
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //healthBar.SetCurrentHealth(currentHealth);
        //_animatorHandler.PlayTargetAnimation("Body Impact", false);

        if (currentHealth <= 0)
        {
            _animatorHandler.PlayTargetAnimation("Dead_side", true);
            var constraints = _rigidbody.constraints;
            constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
            constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
            _rigidbody.constraints = constraints;
        }
    }
}
