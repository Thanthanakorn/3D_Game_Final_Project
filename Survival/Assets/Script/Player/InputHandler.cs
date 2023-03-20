using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool shiftInput;
    public bool ctrlInput;
    public bool rbInput;
    public bool rtInput; 
    public bool jumpInput;
    public bool lockOnInput;
    public bool rightStickRightInput;
    public bool rightStickLeftInput;
    
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    

    private PlayerControls _inputActions;
    private PlayerAttacker _playerAttacker;
    private CameraHandler _cameraHandler;
    private PlayerInventory _playerInventory;
    private PlayerManager _playerManager;
    private PlayerStats _playerStats;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    private void Awake()
    {
        _playerAttacker = GetComponent<PlayerAttacker>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerManager = GetComponent<PlayerManager>();
        _playerStats = GetComponent<PlayerStats>();
        _cameraHandler = FindObjectOfType<CameraHandler>();
    }

    public void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            _inputActions.PlayerMovement.Move.performed += _ => _movementInput = _.ReadValue<Vector2>();
            _inputActions.PlayerMovement.Camera.performed += _ => _cameraInput = _.ReadValue<Vector2>();
            _inputActions.PlayerMovement.LockOnTargetRight.performed += _ => rightStickRightInput = true;
            _inputActions.PlayerMovement.LockOnTargetLeft.performed += _ => rightStickLeftInput = true;
            _inputActions.PlayerActions.LightAttack.performed += _ => rbInput = true;
            _inputActions.PlayerActions.HeavyAttack.performed += _ => rtInput = true;
            _inputActions.PlayerActions.Jump.performed += _ => jumpInput = true;
            _inputActions.PlayerActions.LockOn.performed += _ => lockOnInput = true;
        }

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    public void TickInput()
    {
        HandleMoveInput();
        HandleRollingAndSprintingInput();
        HandleAttackInput();
        HandleLockOnInput();
    }

    private void HandleMoveInput()
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = _cameraInput.x;
        mouseY = _cameraInput.y;
    }

    private void HandleRollingAndSprintingInput()
    {
        ctrlInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        shiftInput = _inputActions.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        rollFlag = ctrlInput;
        sprintFlag = shiftInput;
    }

    private void HandleAttackInput()
    {
        if (rbInput)
        {
            if (_playerManager.canDoCombo)
            {
                comboFlag = true;
                _playerAttacker.HandleWeaponCombo(_playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (_playerManager.isInteracting || _playerManager.canDoCombo)
                {
                    return;
                }
                _playerAttacker.HandleLightAttack(_playerInventory.rightWeapon);
            }
        }

        if (rtInput)
        {
            if (_playerManager.canDoCombo)
            {
                comboFlag = true;
                _playerAttacker.HandleWeaponCombo(_playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
            }
        }
    }
    
    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            _cameraHandler.HandleLockOn(); 
            if (_cameraHandler.nearestLockOnTarget != null)
            {
                _cameraHandler.currentLockOnTarget = _cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnFlag = false;
            lockOnInput = false;
            _cameraHandler.ClearLockOnTargets();
        }

        if (lockOnFlag && rightStickLeftInput)
        {
            _cameraHandler.ClearLockOnSideTargets();
            rightStickLeftInput = false;
            _cameraHandler.HandleLockOn();
            if (_cameraHandler.leftLockTarget != null)
            {
                _cameraHandler.currentLockOnTarget = _cameraHandler.leftLockTarget;
            }
        }

        if (lockOnFlag && rightStickRightInput)
        {
            _cameraHandler.ClearLockOnSideTargets();
            rightStickRightInput = false;
            _cameraHandler.HandleLockOn();
            if (_cameraHandler.rightLockTarget != null)
            {
                _cameraHandler.currentLockOnTarget = _cameraHandler.rightLockTarget;
            }
        }

        _cameraHandler.SetCameraHeight();
    }
}