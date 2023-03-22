using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyStats _enemyStats;
    
    public State currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;
    public bool isPerformingAction;
    private EnemyWeaponSlotManager _weaponSlotManager;
    public WeaponItem rightWeapon;
    public CharacterStats switchToPlayer;
    
    [Header("Enemy Settings")] public float rotationSpeed = 20;
    public float maximumAttackRange = 8f;


    [Header("A.I Settings")] public float detectionRadius = 90;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    
    public float currentRecoveryTime;

    private void Awake()
    {
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyStats = GetComponent<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        _weaponSlotManager = GetComponentInChildren<EnemyWeaponSlotManager>();
        //switchToPlayer = GetComponent<PlayerStats>();
        /*if (switchToPlayer == null)
        {
            Debug.Log("switch to player is null");
        }*/
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
        navMeshAgent.enabled = true;
        _weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
    }

    private void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        if (_enemyStats.isDead) return;
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, _enemyStats, _enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
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

}
