using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) 
    { 
        if (enemyManager.isPerformingAction) return this;

        var currentTargetPosition = enemyManager.currentTarget.transform.position;
        var position = transform.position;
        //Vector3 targetDirection = currentTargetPosition - position;
        enemyManager.distanceFromTarget = Vector3.Distance(currentTargetPosition, position);
        //float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManager.animator.SetFloat(Vertical, 1, 0.1f, Time.deltaTime);
        }

        HandleRotateTowardTarget(enemyManager);
        var navMeshAgentTransform = enemyManager.navMeshAgent.transform;
        navMeshAgentTransform.localPosition = Vector3.zero;
        navMeshAgentTransform.localRotation = Quaternion.identity;

        if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
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
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation,
                enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
