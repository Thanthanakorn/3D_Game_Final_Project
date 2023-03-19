using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private InputHandler _inputHandler;
    private PlayerManager _playerManager;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform _myTransform;
    private Vector3 _cameraTransformPosition;
    private LayerMask _ignoreLayers;
    public LayerMask environmentLayer;
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    public static CameraHandler Singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;
    public float lockedPivotPosition = 9f;
    public float unlockPivotPosition = 8f;
    
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    
    private List<CharacterManager> _availableTargets = new();
    public CharacterManager currentLockOnTarget;
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;
    public float maximumLockOnDistance = 30;

    private void Awake()
    {
        Singleton = this;
        _myTransform = transform;
        _defaultPosition = cameraTransform.localPosition.z;
        _ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        environmentLayer = LayerMask.NameToLayer("Environment");
    }

    public void FollowTarget(float delta)
    {
        var targetPosition = Vector3.SmoothDamp
            (_myTransform.position, targetTransform.position, ref _cameraFollowVelocity, delta / followSpeed);
        _myTransform.position = targetPosition;
        HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (_inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            _lookAngle += (mouseXInput * lookSpeed) / delta;
            _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

            var rotation = Vector3.zero;
            rotation.y = _lookAngle;
            var targetRotation = Quaternion.Euler(rotation);
            _myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = _pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            var position = currentLockOnTarget.transform.position;
            var dir = position - transform.position;
            dir.Normalize();
            dir.y = 0;

            var targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            var eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    private void HandleCameraCollisions(float delta)
    {
        var targetPosition = _defaultPosition;
        RaycastHit hit;
        var direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition)
                , _ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = _cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        var shortestDistance = Mathf.Infinity;
        var shortestDistanceOfLeftTarget = Mathf.Infinity;
        var shortestDistanceOfRightTarget = Mathf.Infinity;

        var colliders = Physics.OverlapSphere(targetTransform.position, 26);

        foreach (var t in colliders)
        {
            var character = t.GetComponent<CharacterManager>();

            if (character != null)
            {
                var position = targetTransform.position;
                var characterPosition = character.transform.position;
                var lockTargetDirection = characterPosition - position;
                var distanceFromTarget = Vector3.Distance(position, characterPosition);
                var viewAbleAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;
                if (character.transform.root != targetTransform.transform.root
                    && viewAbleAngle is > -50 and < 50
                    && distanceFromTarget <= maximumLockOnDistance)
                {
                    if (Physics.Linecast(_playerManager.lockOnTransform.position, character.lockOnTransform.position,
                            out hit))
                    {
                        Debug.DrawLine(_playerManager.lockOnTransform.position, character.lockOnTransform.position);

                        if (hit.transform.gameObject.layer == environmentLayer)
                        {
                            //Cannot lock onto target, object in the way
                        }
                        else
                        {
                            _availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        foreach (var availableTargetPointer in _availableTargets)
        {
            var distanceFromTarget =
                Vector3.Distance(targetTransform.position, availableTargetPointer.transform.position);

            if (distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargetPointer;
            }

            if (_inputHandler.lockOnFlag)
            {
                //var availableTargetPosition = availableTargetPointer.transform.position;
                //var currentLockOnTargetPosition = currentLockOnTarget.transform.position;
                var relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargetPointer.transform.position);
                var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargetPointer.transform.position.x;
                var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargetPointer.transform.position.x;

                if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTargetPointer;
                }

                if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTargetPointer;
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        _availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
        rightLockTarget = null;
        leftLockTarget = null;
    }

    public void ClearLockOnSideTargets()
    {
        rightLockTarget = null;
        leftLockTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockPivotPosition);

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}