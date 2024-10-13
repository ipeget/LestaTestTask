using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform CinemachineCameraTarget;
    [SerializeField] private float TopClamp = 70.0f;
    [SerializeField] private float BottomClamp = -30.0f;
    [SerializeField] private float CameraAngleOverride = 0.0f;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private Vector2 look;
    private const float threshold = 0.01f;

    private void Start()
    {
        cinemachineTargetYaw = !CinemachineCameraTarget ? 0 : CinemachineCameraTarget.rotation.eulerAngles.y;
    }

    private void Update()
    {
        InputHandler();
    }

    public void LateUpdate()
    {
        if (CinemachineCameraTarget)
            CameraRotation();
    }

    private void CameraRotation()
    {
        if (look.sqrMagnitude >= threshold)
        {
            cinemachineTargetYaw += look.x;
            cinemachineTargetPitch += look.y;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void InputHandler()
    {
        look = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
    }
}
