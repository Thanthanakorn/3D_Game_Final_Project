using System;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    private EnemyLocomotionManager _enemyLocomotionManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
    }

    private void OnAnimatorMove()
    {
        var delta = Time.deltaTime;
        _enemyLocomotionManager.enemyRigidbody.drag = 0;
        var deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        var velocity = deltaPosition / delta;
        _enemyLocomotionManager.enemyRigidbody.velocity = velocity;
    }
}
