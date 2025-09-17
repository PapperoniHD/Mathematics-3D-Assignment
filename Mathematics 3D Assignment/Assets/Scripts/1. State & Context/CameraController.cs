using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBody;

    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float smoothing = 10f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distanceFromPlayer = 30f;

    float yaw = 0f;
    float pitch = 0f;

    float smoothYaw;
    float smoothPitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MouseMovement();
    }

    void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        smoothYaw = MathUtility.Lerp(smoothYaw, yaw, Time.deltaTime * smoothing);
        smoothPitch = MathUtility.Lerp(smoothPitch, pitch, Time.deltaTime * smoothing);

        playerBody.transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);

        transform.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);

        cameraTransform.localPosition = new Vector3(0, 0, -distanceFromPlayer);
        cameraTransform.localRotation = Quaternion.identity;

    }

}
