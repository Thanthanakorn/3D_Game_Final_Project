using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponItem attackingWeapon;
    
    private WeaponHolderSlot _leftHandSlot;
    private WeaponHolderSlot _rightHandSlot;

    private DamageCollider _leftHandDamageCollider;
    private DamageCollider _rightHandDamageCollider;

    private Animator _animator;

    private PlayerStats _playerStats;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerStats = GetComponentInParent<PlayerStats>();
        
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                _leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                _rightHandSlot = weaponSlot;
            }
        }
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            _leftHandSlot.currentWeapon = weaponItem;
            _leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            
            if (weaponItem != null)
            {
                _animator.CrossFade(weaponItem.standPose, 0.2f);
            }
            else 
                _animator.CrossFade("Left Arm Empty", 0.2f);
        }
        else
        {
            _rightHandSlot.currentWeapon = weaponItem;
            _rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            if (weaponItem != null)
            {
                _animator.CrossFade(weaponItem.standPose, 0.2f);
            }
            else 
                _animator.CrossFade("Left Arm Empty", 0.2f);
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadRightWeaponDamageCollider()
    {
        _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadLeftWeaponDamageCollider()
    {
        _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }


    public void OpenDamageCollider()
    {
        _rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        _rightHandDamageCollider.DisableDamageCollider();
    }

    #endregion
    
    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    
    public void DrainStaminaHeavyAttack()
    {
        _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}
