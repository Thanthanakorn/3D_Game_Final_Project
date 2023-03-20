using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public DeadState deadState;
    
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) 
    { 
        if (enemyStats.isDead)
        {
            return deadState;
        }
        
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.animator.SetFloat(Vertical, 0, 0.1f, Time.deltaTime);
            return this;
        }

        var currentTargetPosition = enemyManager.currentTarget.transform.position;
        var position = enemyManager.transform.position;
        Vector3 targetDirection = currentTargetPosition - position;
        float distanceFromTarget = Vector3.Distance(currentTargetPosition, position);
        //float viewAbleAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManager.animator.SetFloat(Vertical, 1, 0.1f, Time.deltaTime);
        }

        HandleRotateTowardTarget(enemyManager);
        var navMeshAgentTransform = enemyManager.navMeshAgent.transform;
        navMeshAgentTransform.localPosition = Vector3.zero;
        navMeshAgentTransform.localRotation = Quaternion.identity;

        if ( distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
        }

        return this;
    }
    
    private void HandleRotateTowardTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            var direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        else
        {
            //var relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            var targetVelocity = enemyManager.enemyRigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation,
                enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
