using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private AnimatorHandler _animatorHandler;
    private InputHandler _inputHandler;
    private WeaponSlotManager _weaponSlotManager;
    private PlayerManager _playerManager;
    private PlayerStats _playerStats;
    private PlayerInventory _playerInventory;
    private PlayerEquipmentManager _playerEquipmentManager;
    
    public string lastAttack;
    private static readonly int CanDoCombo = Animator.StringToHash("canDoCombo");

    private void Awake()
    {
        _animatorHandler = GetComponentInChildren< AnimatorHandler>();
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        _inputHandler = GetComponent<InputHandler>();
        _playerStats = GetComponent<PlayerStats>();
        _playerManager = GetComponent<PlayerManager>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (_playerStats.isDead || _playerStats.currentStamina <= (weapon.baseStamina * weapon.lightAttackMultiplier) || _playerManager.isAttacking) return;
        _weaponSlotManager.attackingWeapon = weapon;
        _animatorHandler.PlayTargetAttackingAnimation(weapon.ohLightAttack1, true);
        lastAttack = weapon.ohLightAttack1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (_playerStats.isDead || _playerStats.currentStamina <= (weapon.baseStamina * weapon.heavyAttackMultiplier) || _playerManager.isAttacking) return;
        _weaponSlotManager.attackingWeapon = weapon;
        _animatorHandler.PlayTargetAttackingAnimation(weapon.ohHeavyAttack1, true);
        lastAttack = weapon.ohHeavyAttack1;
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (_inputHandler.comboFlag)
        {
            if (_playerStats.isDead || _playerStats.currentStamina <= 0) return;
            
            ((AnimatorManager)_animatorHandler).animator.SetBool(CanDoCombo, false);
            
            if (lastAttack == weapon.ohLightAttack1)
            {
                _animatorHandler.PlayTargetAttackingAnimation(weapon.ohLightAttack2, true);
                lastAttack = weapon.ohLightAttack2;
            }
            else if (lastAttack == weapon.ohLightAttack2)
            {
                _animatorHandler.PlayTargetAttackingAnimation(weapon.ohLightAttack3, true);
            }
            else if (lastAttack == weapon.ohHeavyAttack1)
            {
                _animatorHandler.PlayTargetAttackingAnimation(weapon.ohHeavyAttack2, true);
                lastAttack = weapon.ohHeavyAttack2;
            }
            else if (lastAttack == weapon.ohHeavyAttack2)
            {
                _animatorHandler.PlayTargetAttackingAnimation(weapon.ohHeavyAttack3, true);
            }
        }
    }
    
    public void HandleParry(WeaponItem weapon)
    {
        if (_playerManager.isInteracting || _playerManager.isAttacking) return;
        _animatorHandler.PlayTargetAnimation(_playerInventory.leftWeapon.parry, true);
    }

    public void HandleBlock()
    {
        PerformBlockingAction();
    }
    
    #region Defense Actions

    private void PerformBlockingAction()
    {
        if (_playerManager.isInteracting) return;
        
        if(_playerManager.isBlocking) return;
        
        _animatorHandler.PlayTargetAnimation("Blocking 1", false);
        _playerManager.isBlocking = true;
    }
    
    #endregion
}
