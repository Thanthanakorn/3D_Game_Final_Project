using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsUI : MonoBehaviour
{
    public CameraHandler cameraHandler;
    public Slider lookSpeedSlider;
    public Slider followSpeedSlider;
    public Slider pivotSpeedSlider;

    void Start()
    {
        lookSpeedSlider.value = cameraHandler.lookSpeed;
        followSpeedSlider.value = cameraHandler.followSpeed;
        pivotSpeedSlider.value = cameraHandler.pivotSpeed;

        lookSpeedSlider.onValueChanged.AddListener(UpdateLookSpeed);
        followSpeedSlider.onValueChanged.AddListener(UpdateFollowSpeed);
        pivotSpeedSlider.onValueChanged.AddListener(UpdatePivotSpeed);
    }

    void UpdateLookSpeed(float value)
    {
        cameraHandler.lookSpeed = value;
    }

    void UpdateFollowSpeed(float value)
    {
        cameraHandler.followSpeed = value;
    }

    void UpdatePivotSpeed(float value)
    {
        cameraHandler.pivotSpeed = value;
    }
}