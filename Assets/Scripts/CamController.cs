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

        camDelegate = InputsFunct;
        camDelegate += FPSCamMovement;
    }
    private void LateUpdate()
    {
        camDelegate?.Invoke();
    }

    void InputsFunct()
    {
        Debug.Log(transform.localEulerAngles.y);
        if (Input.GetButtonDown("Right"))
        {
            StartCoroutine(RotatePlayer(90));
        }
        if (Input.GetButtonDown("Left"))
        {
            StartCoroutine(RotatePlayer(-90));
        }
    }

    IEnumerator RotatePlayer(float degrees)
    {
        camDelegate -= InputsFunct;
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + degrees, 0);

        do
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, _90DegreesRotationSpeed * Time.deltaTime);
            yield return null;
        }
        while (Mathf.Abs(Quaternion.Angle(player.transform.rotation, targetRotation)) > .5f);
        
        player.transform.rotation = targetRotation;
        camDelegate += InputsFunct;
    }

    void FPSCamMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * FPSRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * FPSRotationSpeed;

        yRotation -= mouseY;
        xRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -45, 45);
        yRotation = Mathf.Clamp(yRotation, -45, 45);

        transform.localEulerAngles= new Vector3(yRotation, xRotation, 0);
    }
}