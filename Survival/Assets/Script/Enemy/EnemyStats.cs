using System;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public UIEnemyHealthBar healthBar;
    private EnemyAnimatorManager _animatorHandler;
    
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private EnemyManager _enemyManager;
    
    public GameObject damageEffectPrefab;
    public Transform damageEffectPosition;



    public bool getHit;

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyManager = GetComponent<EnemyManager>();
        isDead = false;
        getHit = false;
    }

    private void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        maxHealth = SetMaxHealthFromHealthLevel();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
    
    private float SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(float damage, string damageAnimation = "Body Impact")
    {
        if (isDead) return;
        
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            _rigidbody = GetComponentInParent<Rigidbody>();
            _collider = GetComponentInParent<CapsuleCollider>();
            _animatorHandler.PlayTargetAnimation("Dead_side", true);
            var constraints = _rigidbody.constraints;
            constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
            constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
            _rigidbody.constraints = constraints;
            GameObject o;
            (o = gameObject).layer = LayerMask.NameToLayer("Ignore Raycast");
            Destroy(o,8f);
        }
        else
        {
            getHit = true;
            GameObject damageEffect = Instantiate(damageEffectPrefab, damageEffectPosition.position, damageEffectPosition.rotation);
            damageEffect.transform.SetParent(transform);
            Destroy(damageEffect, 1f);
            _animatorHandler.PlayTargetAnimation(damageAnimation, true);
        }
    }

   
}
