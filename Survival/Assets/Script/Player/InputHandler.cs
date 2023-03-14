using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    private PlayerControls _inputActions;
    
    

    private Vector2 _movementInput;
    Vector2 _cameraInput;

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

    public void TickInput(float delta)
    {
        MoveInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = _cameraInput.x;
        mouseY = _cameraInput.y;
    }
}