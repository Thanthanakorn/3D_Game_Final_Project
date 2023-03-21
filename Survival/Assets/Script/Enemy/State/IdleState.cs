using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public DeadState deadState;
    public LayerMask detectionLayer;
    
    public float range;
    public Transform centrePoint;
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,
        EnemyAnimatorManager enemyAnimatorManager)
    {
        
        if (enemyStats.isDead)
        {
            return deadState;
        }
        

        #region Handle Patrolling
        if(enemyManager.navMeshAgent.remainingDistance <= enemyManager.navMeshAgent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); 
                enemyManager.navMeshAgent.SetDestination(point);
            }
            
            var currentTargetPosition = enemyManager.navMeshAgent.transform.position;
            var position = enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(currentTargetPosition, position);
            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorManager.animator.SetFloat(Vertical, 2, 0.1f, Time.deltaTime);
            }
            var targetVelocity = enemyManager.enemyRigidbody.velocity;
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation,
                enemyManager.rotationSpeed / Time.deltaTime);
        }
        #endregion

        #region Handle Enemy Target Detection

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.minimumDetectionAngle &&
                    viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }
        #endregion

        #region Handle Swtiching To Next State
        
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        
        #endregion
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f,
                NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}