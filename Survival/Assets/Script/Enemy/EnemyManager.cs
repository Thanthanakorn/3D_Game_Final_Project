using UnityEngine;

public class EnemyManager : CharacterManager
{
    private EnemyLocomotionManager _enemyLocomotionManager;
    private EnemyAnimatorManager _enemyAnimatorManager;
    public bool isPerformingAction;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    
    [Header("Enemy Settings")]
    public float detectionRadius = 20;

    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;
    private void Awake()
    {
        _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    private void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction() 
    {
        if (_enemyLocomotionManager.currentTarget != null)
        {
            _enemyLocomotionManager.distanceFromTarget =
                Vector3.Distance(_enemyLocomotionManager.currentTarget.transform.position, transform.position);
        }
            
        if (_enemyLocomotionManager.currentTarget == null)
        {
            _enemyLocomotionManager.HandleDetection();
        }
        else if (_enemyLocomotionManager.distanceFromTarget > _enemyLocomotionManager.stoppingDistance)
        {
            _enemyLocomotionManager.HandleMoveToTarget();
        }
        else if (_enemyLocomotionManager.distanceFromTarget <= _enemyLocomotionManager.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    #region Attacks

    private void AttackTarget()
    {
        if (isPerformingAction)
            return;
        
        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isPerformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            _enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            currentAttack = null;
        }
    }

    private void GetNewAttack()
    {
        var targetDirection = _enemyLocomotionManager.currentTarget.transform.position - transform.position;
        var viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        _enemyLocomotionManager.distanceFromTarget = Vector3.Distance(_enemyLocomotionManager.currentTarget.transform.position, transform.position);

        var maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            var enemyAttackAction = enemyAttacks[i];
            if (_enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && _enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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
            if (_enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && _enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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

    #endregion
}
