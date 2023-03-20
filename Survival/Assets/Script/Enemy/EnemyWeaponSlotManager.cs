using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot _rightHandSlot;
    private WeaponHolderSlot _leftHandSlot;

    private DamageCollider _rightHandDamageCollider;

    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
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
                if (_rightHandSlot == null)
                {
                    Debug.Log("right hand slot is null");
                }
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem)
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

    #region Handle Weapon's Damage Collider

    private void LoadRightWeaponDamageCollider()
    {
        _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        
        if (_rightHandDamageCollider == null)
        {
            Debug.Log("right hand damage collider is null");
        }
    }

    public void EnemyOpenDamageCollider()
    {
        if (_rightHandDamageCollider == null)
        {
            Debug.Log("right hand damage collider is null");
        }
        _rightHandDamageCollider.EnableDamageCollider();
    }

    public void EnemyCloseDamageCollider()
    {
        _rightHandDamageCollider.DisableDamageCollider();
    }

    #endregion
}