using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;
    public DeadState deadState;
    
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,
        EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.isDead)
        {
            return deadState;
        }
        
        var transform1 = transform;
        var currentTargetPosition = enemyManager.currentTarget.transform.position;
        Vector3 targetDirection = currentTargetPosition - transform1.position;
        float distanceFromTarget =
            Vector3.Distance(currentTargetPosition, enemyManager.transform.position);
        float viewAbleAngle = Vector3.Angle(targetDirection, transform1.forward);
        if (enemyManager.isPerformingAction)
            return combatStanceState;
        
        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
            {
                if (viewAbleAngle <= currentAttack.maximumAttackAngle &&
                    viewAbleAngle >= currentAttack.minimumAttackAngle)
                {
                    if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                    {
                        enemyAnimatorManager.animator.SetFloat(Vertical, 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.animator.SetFloat(Horizontal, 0,0.1f,Time.deltaTime);
                        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }
        return combatStanceState;
    }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        var currentTargetPosition = enemyManager.currentTarget.transform.position;
        var transform1 = transform;
        var position = transform1.position;
        var targetDirection = currentTargetPosition - position;
        var viewableAngle = Vector3.Angle(targetDirection, transform1.forward);
        float distanceFromTarget = Vector3.Distance(currentTargetPosition, position);

        var maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            var enemyAttackAction = enemyAttacks[i];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        var randomValue = Random.Range(0, maxScore);
        var temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            var enemyAttackAction = enemyAttacks[i];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (currentAttack != null) return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }

    }
}
