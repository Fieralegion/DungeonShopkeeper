using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float rotationSpeed;
    Quaternion targetRotation;
    Coroutine rotationCoroutine;

    public void Start()
    {
        targetRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button14)) Debug.Log(KeyCode.Joystick1Button14);

        if (Input.GetButtonDown("Right") /*|| Input.mousePosition.x >= Screen.width * .1f*/)
        {
            RotateCam(90);
        }
        if (Input.GetButtonDown("Left") /*|| Input.mousePosition.x <= Screen.width * (1 - .1f)*/)
        {
            RotateCam(-90);
        }
    }

    void RotateCam(float degrees)
    {
        // Spin our target rotation in the desired direction.
        targetRotation = Quaternion.Euler(0, degrees, 0) * targetRotation;

        // If we're not already rotating, start a new rotation.
        // (Otherwise, the coroutine will automatically handle the new target)
        if (rotationCoroutine == null)
            rotationCoroutine = StartCoroutine(RotationCoroutine());

        IEnumerator RotationCoroutine()
        {
            // Until our current rotation aligns with the target...
            while (Quaternion.Dot(transform.rotation, targetRotation) < 1f)
            {
                // Rotate at a consistent speed toward the target.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Wait a frame, then resume.
                yield return null;
            }

            // Clear the coroutine so the next input starts a fresh one.
            rotationCoroutine = null;
        }
    }

}