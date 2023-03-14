using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    public bool bInput;
    public bool rollFlag;
    public bool isInteracting;

    private PlayerControls _inputActions;
    private CameraHandler _cameraHandler;
    
    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    private void Awake()
    {
        _cameraHandler = CameraHandler.Singleton;
    }

    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;

        if (_cameraHandler == null) return;
        _cameraHandler.FollowTarget(delta);
        _cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
    }

    public void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            _inputActions.PlayerMovement.Move.performed +=
                inputActions => _movementInput = inputActions.ReadValue<Vector2>();
            _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
        }

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollingInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = _cameraInput.x;
        mouseY = _cameraInput.y;
    }

    private void HandleRollingInput(float delta)
    {
        bInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if (bInput)
        {
            Debug.Log("Rolling");
            rollFlag = true;
        }
    }
}