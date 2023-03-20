using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);

        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat(Vertical, 0,0.1f, Time.deltaTime);
        }
        
        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }

        return this;
    }
}
