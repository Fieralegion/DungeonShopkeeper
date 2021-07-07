using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject player; // mover a libreria
    [SerializeField] private float _90DegreesRotationSpeed;
    Quaternion targetRotation;

    [SerializeField] private float FPSRotationSpeed;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private delegate void CamDelegate();
    private CamDelegate camDelegate;

    public void Awake()
    {
        targetRotation = player.transform.rotation;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camDelegate += FPSCamMovement;
    }
    private void LateUpdate()
    {
        camDelegate?.Invoke();
    }

    void FPSCamMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * FPSRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * FPSRotationSpeed;

        yRotation -= mouseY;
        xRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -80, 80);

        transform.localEulerAngles= new Vector3(yRotation, xRotation, 0);
    }
}