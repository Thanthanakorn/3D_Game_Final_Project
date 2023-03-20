using UnityEngine;

public class PlayerManager : CharacterManager
{
    private InputHandler _inputHandler;
    private Animator _anim;
    private CameraHandler _cameraHandler;
    private PlayerLocomotion _playerLocomotion;
    private PlayerStats _playerStats;
    
    
    public bool isInteracting;
    public bool isAttacking;
    
    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isInvulnerable;

    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int CanDoCombo = Animator.StringToHash("canDoCombo");
    private static readonly int IsInAir = Animator.StringToHash("isInAir");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsInvulnerable = Animator.StringToHash("isInvulnerable");

    private void Awake()
    {
        _cameraHandler = CameraHandler.Singleton;
    }

    private void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _anim = GetComponentInChildren<Animator>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = _anim.GetBool(IsInteracting);
        isAttacking = _anim.GetBool(IsAttacking);
        canDoCombo = _anim.GetBool(CanDoCombo);
        _anim.SetBool(IsInAir, isInAir);
        _anim.GetBool(IsInvulnerable);
        _inputHandler.TickInput();
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerLocomotion.HandleJumping();
        _anim.SetBool(IsGrounded,isGrounded);
        _playerStats.RegenerateStamina();
    }
    
    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleFalling(delta, _playerLocomotion.moveDirection);
    }

    private void LateUpdate()
    {
        _inputHandler.rollFlag = false;
        _inputHandler.rtInput = false;
        _inputHandler.rbInput = false;
        _inputHandler.jumpInput = false;
        
        var delta = Time.deltaTime;
        
        if (_cameraHandler == null) return;
        _cameraHandler.FollowTarget(delta);
        _cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);

        if (isInAir)
        {
            _playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }
}
