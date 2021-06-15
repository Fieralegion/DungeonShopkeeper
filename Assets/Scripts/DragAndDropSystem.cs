using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    GameObject getTarget;
    bool isDragged;
    float distance;

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDragg();
        }

        if (isDragged)
        {
            Dragged(ref distance);
        }
    }

    void BeginDrag()
    {
        getTarget = ReturnClickedObject(out RaycastHit hit);
        if (getTarget != null)
        {
            isDragged = true;
            distance = Vector3.Distance(Camera.main.transform.position, getTarget.transform.position);
        }
    }

    void Dragged(ref float dist)
    {
        dist += Input.mouseScrollDelta.y;
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);

        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace);

        getTarget.transform.position = currentPosition;
    }

    void EndDragg()
    {
        isDragged = false;
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
