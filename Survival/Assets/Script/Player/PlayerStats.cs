using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Rigidbody _rigidbody;
    public bool isDead;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    private AnimatorHandler _animatorHandler;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        _rigidbody = GetComponent<Rigidbody>();
        isDead = false;
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        _animatorHandler.PlayTargetAnimation("Body Impact", true);

        if (currentHealth <= 0)
        {
            _animatorHandler.PlayTargetAnimation("Dead Forward", true);
            var constraints = _rigidbody.constraints;
            constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
            constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
            _rigidbody.constraints = constraints;
            _animatorHandler.StopRotation();
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina -= damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}
