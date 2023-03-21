using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEquipmentManager : MonoBehaviour
{
    public BlockingCollider blockingCollider;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponentInParent<PlayerInventory>();
    }

    public void OpenBlockingCollider()  
    {
        blockingCollider.SetColliderDamageAbsorption(_playerInventory.leftWeapon);
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
