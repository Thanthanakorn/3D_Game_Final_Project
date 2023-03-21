using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    private EnemyManager _enemyManager;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    private void OnAnimatorMove()
    {
        var delta = Time.deltaTime;

        if (Mathf.Approximately(delta, 0f))
        {
            return;
        }
        
        _enemyManager.enemyRigidbody.drag = 0;
        var deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        var velocity = deltaPosition / delta;

        if (!float.IsNaN(velocity.x) && !float.IsNaN(velocity.y) && !float.IsNaN(velocity.z))
        {
            _enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}