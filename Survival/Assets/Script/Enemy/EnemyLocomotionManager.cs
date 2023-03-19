using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private EnemyAnimatorManager _enemyAnimationManager;
    private NavMeshAgent _navMeshAgent;
    public Rigidbody enemyRigidbody;
    
    public CharacterStats currentTarget;
    public LayerMask detectionLayer;

    public float distanceFromTarget;
    public float stoppingDistance = 1;
    public float rotationSpeed = 20;
    
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _enemyManager = GetComponent<EnemyManager>();
        _enemyAnimationManager = GetComponentInChildren<EnemyAnimatorManager>();
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _navMeshAgent.enabled = false;
        enemyRigidbody.isKinematic = false;
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > _enemyManager.minimumDetectionAngle &&
                    viewableAngle < _enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        if (_enemyManager.isPerformingAction) return;
        
        var targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        var viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

        //If we are performing an action, stop our movement!
        if (_enemyManager.isPerformingAction)
        {
            _enemyAnimationManager.animator.SetFloat(Vertical, 0, 0.1f, Time.deltaTime);
            _navMeshAgent.enabled = false;
        }
        else
        {
            if (distanceFromTarget > stoppingDistance)
            {
                _enemyAnimationManager.animator.SetFloat(Vertical, 1, 0.1f, Time.deltaTime);
            }
            else if (distanceFromTarget <= stoppingDistance)
            {
                _enemyAnimationManager.animator.SetFloat(Vertical, 0, 0.1f, Time.deltaTime);
            }
        }
        HandleRotateTowardTarget();
        
        var navMeshAgentTransform = _navMeshAgent.transform;
        navMeshAgentTransform.localPosition = Vector3.zero;
        navMeshAgentTransform.localRotation = Quaternion.identity;
    }

    private void HandleRotateTowardTarget()
    {
        //Rotate manually
        if (_enemyManager.isPerformingAction)
        {
            var direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
        else
        {
            var relativeDirection = transform.InverseTransformDirection(_navMeshAgent.desiredVelocity);
            var targetVelocity = enemyRigidbody.velocity;

            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, _navMeshAgent.transform.rotation,
                rotationSpeed / Time.deltaTime);
        }
    }
}
