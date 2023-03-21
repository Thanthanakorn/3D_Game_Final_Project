using UnityEngine;

public class DeadState : State
{
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.PlayTargetAnimation("Dead_side", true);
        return this;
    }
}
