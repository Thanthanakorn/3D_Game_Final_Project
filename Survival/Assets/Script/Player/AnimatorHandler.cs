using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    private PlayerManager _playerManager;
    private PlayerLocomotion _playerLocomotion;
    private int _vertical;
    private int _horizontal;
    public bool canRotate;

    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int CanDoCombo = Animator.StringToHash("canDoCombo");
    private static readonly int IsInvulnerable = Animator.StringToHash("isInvulnerable");

    public void Initialize()
    {
        animator = GetComponent<Animator>();
        _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        _playerManager = GetComponentInParent<PlayerManager>();
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical

        float v = verticalMovement switch
        {
            > 0 and < 0.55f => 0.5f,
            > 0.55f => 1,
            < 0 and > -0.55f => -0.5f,
            < -0.55f => -1,
            _ => 0
        };

        #endregion

        #region Horizontal

        var h = horizontalMovement switch
        {
            > 0 and < 0.55f => 1,
            < 0 and > -0.55f => -0.5f,
            < -0.55f => -1,
            _ => 0
        };

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        animator.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    private void OnAnimatorMove()
    {
        if (_playerManager.isAttacking == false)
            return;
    
        var delta = Time.deltaTime;
        _playerLocomotion.rigidbody.drag = 0;
        var deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        var velocity = deltaPosition / delta;
        _playerLocomotion.rigidbody.velocity = velocity;
    }
    
    public void PlayTargetAttackingAnimation(string targetAnim, bool isAttacking)
    {
        animator.applyRootMotion = isAttacking;
        animator.SetBool(IsAttacking, isAttacking);
        animator.CrossFade(targetAnim, 0.2f);
        
        if (isAttacking)
        {
            _playerLocomotion.ResetVelocity();
        }
    }

    public void EnableCombo()
    {
        animator.SetBool(CanDoCombo, true);
    }

    public void DisableCombo()
    {
        animator.SetBool(CanDoCombo, false);
    }

    public void EnableIsInteracting()
    {
        animator.SetBool(IsInteracting,true);
    }
    
    public void DisableIsInteracting()
    {
        animator.SetBool(IsInteracting, false);
    }
    
    public void EnableIsAttacking()
    {
        animator.SetBool(IsAttacking, true);
    }
    
    public void DisableIsAttacking()
    {
        animator.SetBool(IsAttacking, false);
    }

    public void EnableIsInvulnerable()
    {
        animator.SetBool(IsInvulnerable, true);
    }

    public void DisableIsInvulnerable()
    {
        animator.SetBool(IsInvulnerable, false);
    }
}