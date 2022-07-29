using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class CamController : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float _90DegreesRotationSpeed;

    [SerializeField] private float FPSRotationSpeed;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private delegate void CamDelegate();
    private CamDelegate camDelegate;

    public void Start()
    {
        GameSettings.DisableMouse();

        camDelegate += FPSCamMovement;
    }
    private void LateUpdate()
    {
        camDelegate?.Invoke();
    }

    void FPSCamMovement()
    {
        if (Time.timeScale == 1)
        {
            float mouseX = Input.GetAxis("Mouse X") * FPSRotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * FPSRotationSpeed;

            yRotation -= mouseY;
            xRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -80, 80);

            transform.localEulerAngles = new Vector3(yRotation, xRotation, 0);
        }
        if (GameSettings.gamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}