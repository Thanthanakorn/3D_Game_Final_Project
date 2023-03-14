 using UnityEngine;

 public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    public InputHandler inputHandler;
    public PlayerLocomotion playerLocomotion;
    private int _vertical;
    private int _horizontal;
    public bool canRotate;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
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

        float h = horizontalMovement switch
        {
            > 0 and < 0.55f => 1,
            < 0 and > -0.55f => -0.5f,
            < -0.55f => -1,
            _ => 0
        };

        #endregion
        
        anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
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
        if (inputHandler.isInteracting == false)
            return;
        var delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        var deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        var velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        Debug.Log("Rolling PlayTargetAnimation");
        anim.applyRootMotion = isInteracting;
        anim.SetBool(IsInteracting, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
}
