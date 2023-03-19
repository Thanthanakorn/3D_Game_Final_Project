using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);

        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }

        return this;
    }
}
