using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    private EnemyLocomotionManager _enemyLocomotionManager;
    private bool _isPerformingAction;
    
    [Header("Enemy Settings")]
    public float detectionRadius = 20;

    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    private void Awake()
    {
        _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }

    private void Update()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (_enemyLocomotionManager.currentTarget == null)
        {
            _enemyLocomotionManager.HandleDetection();
        }
    }
}
