using UnityEngine;
using UnityEngine.PlayerLoop;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int CanRotate = Animator.StringToHash("canRotate");

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(IsInteracting, isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
        
    }
}