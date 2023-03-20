using UnityEngine;

public class DeadState : State
{
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    /*private void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _collider = GetComponentInParent<CapsuleCollider>();
    }*/

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        /*_rigidbody = GetComponentInParent<Rigidbody>();
        _collider = GetComponentInParent<CapsuleCollider>();*/
        enemyAnimatorManager.PlayTargetAnimation("Dead_side", true);
        //_collider.height = 1;
        //var constraints = _rigidbody.constraints;
        //constraints |= RigidbodyConstraints.FreezePositionX; // Freeze position along the y-axis
        //constraints |= RigidbodyConstraints.FreezePositionZ; // Freeze position along the z-axis
        //_rigidbody.constraints = constraints;
        return this;
    }
}
