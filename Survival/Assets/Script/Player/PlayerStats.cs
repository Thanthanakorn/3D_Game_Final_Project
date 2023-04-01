using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Rigidbody _rigidbody;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    private AnimatorHandler _animatorHandler;
    private CapsuleCollider _collider;
    private PlayerManager _playerManager;
    public float staminaRegenerationAmount = 25;
    public float staminaRegenerationTimer = 0;
    
    public GameObject damageEffectPrefab;
    public Transform damageEffectPosition;
    private float _originalYPosition;



    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
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

    private float SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(float damage, string damageAnimation = "Body Impact")
    {
        if (_playerManager.isInvulnerable) return;
        if (isDead) return;
        currentHealth -= damage;
        GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthBar.SetCurrentHealth(currentHealth);
        GameObject damageEffect = Instantiate(damageEffectPrefab, damageEffectPosition.position, damageEffectPosition.rotation);
        damageEffect.transform.SetParent(transform);
        Destroy(damageEffect, 1f);

        _animatorHandler.PlayTargetAnimation(damageAnimation, true);
        if (currentHealth <= 0)
        {
            isDead = true;
            _animatorHandler.PlayTargetAnimation("Dead Forward", true);
            var constraints = _rigidbody.constraints;
            constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
            constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
            _rigidbody.constraints = constraints;
            _animatorHandler.StopRotation();
            _collider.height = 0f;
            _collider.radius = 2.5f;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            return;
        }

        if (currentStamina >= damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }
    }

    public void RegenerateStamina()
    {

        if (_playerManager.isInteracting || _playerManager.isAttacking || _playerManager.isSprinting)
        {
            staminaRegenerationTimer = 0;
        }
        else
        {
            staminaRegenerationTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }
    public void LevelUp()
    {
        healthLevel += 10;
        staminaLevel += 10;
        attackLevel += 5;
        staminaRegenerationAmount += 5;

        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

}