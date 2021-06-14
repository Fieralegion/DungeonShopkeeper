using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    GameObject getTarget;
    bool isDragged;
    Vector3 offset;
    Vector3 positionOfScreen;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragged = false;
        }

        if (isDragged)
        {
            Dragged();
        }
    }

    void Drag()
    {
        getTarget = ReturnClickedObject(out RaycastHit hit);
        if (getTarget != null)
        {
            isDragged = true;

            positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
            offset = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
        }
    }

    void Dragged()
    {
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

        getTarget.transform.position = currentPosition;
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (hit.collider.CompareTag("Draggable"))
            {
                target = hit.collider.gameObject;
            }
        }
        return target;
    }
}
