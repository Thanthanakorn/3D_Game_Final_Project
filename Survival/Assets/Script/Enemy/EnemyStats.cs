using UnityEngine;

public class EnemyStats : CharacterStats
{
    
    public HealthBar healthBar;
    private EnemyAnimatorManager _animatorHandler;
    
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren<EnemyAnimatorManager>();
        isDead = false;
    }

    private void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
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
        if (isDead) return;
        
        currentHealth -= damage;
        //healthBar.SetCurrentHealth(currentHealth);
        //_animatorHandler.PlayTargetAnimation("Body Impact", false);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            _rigidbody = GetComponentInParent<Rigidbody>();
            _collider = GetComponentInParent<CapsuleCollider>();
            _animatorHandler.PlayTargetAnimation("Dead_side", true);
            _collider.height = 1;
            var constraints = _rigidbody.constraints;
            constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
            constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
            _rigidbody.constraints = constraints;
        }
    }
    
}
