using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Transform _cameraObject;
    private InputHandler _inputHandler;
    private Vector3 _moveDirection;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;
    
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")] [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 10;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        if (Camera.main != null) _cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        
        _inputHandler.TickInput(delta);

        _moveDirection = _cameraObject.forward * _inputHandler.vertical;
        _moveDirection += _cameraObject.right * _inputHandler.horizontal;
        _moveDirection.Normalize();
        _moveDirection.y = 0;
        
        float speed = movementSpeed;
        _moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
        
        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
        
    }

    #region Movement

    private Vector3 _normalVector;
    private Vector3 _targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir;
        float moveOverride = _inputHandler.moveAmount;

        targetDir = _cameraObject.forward * _inputHandler.vertical;
        targetDir += _cameraObject.right * _inputHandler.horizontal;
        
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    #endregion
}
