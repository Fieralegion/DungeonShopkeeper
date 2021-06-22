using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    private GameObject getTarget;
    private bool isDragged;
    private Item typeOf;
    [SerializeField] private float scrollSpeed;
    
    bool canBeAttached;
    Transform attachmentTransform;
    Color originalColor;
    HookChecker hookChecker;

    private delegate void DadDelegate(RaycastHit hit);
    private DadDelegate dadDelegate;

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginDrag();
        }

        if (isDragged)
        {
            Dragged();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDragg();
        }
    }

    void BeginDrag()
    {
        getTarget = ReturnClickedObject(out RaycastHit hit);
     
        if (getTarget != null)
        {
            isDragged = true;
            getTarget.transform.parent = null;
            originalColor = getTarget.GetComponent<Renderer>().material.color;
            getTarget.GetComponent<Rigidbody>().isKinematic = true;
            getTarget.GetComponent<Rigidbody>().freezeRotation = true;
            typeOf = getTarget.GetComponent<Item>();
            SwitchSelector();
        }
    }

    void Dragged()
    {
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.25f);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace);
        getTarget.transform.position = currentPosition;
        getTarget.transform.LookAt(Camera.main.transform.position);

        if (Physics.Raycast(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back), out RaycastHit hit))
        {
            dadDelegate?.Invoke(hit);
        }
    }

    void EndDragg()
    {
        isDragged = false;
    
        if (getTarget != null)
        {
            getTarget.GetComponent<Rigidbody>().freezeRotation = false;
            getTarget.GetComponent<Renderer>().material.color = originalColor;

            switch (typeOf._item)
            {
                case Item.TypeOfItem.Product:
                    if (canBeAttached && hookChecker.transform.childCount == 0)
                    {
                        getTarget.transform.SetPositionAndRotation(attachmentTransform.position, attachmentTransform.rotation);
                        getTarget.transform.position += attachmentTransform.forward.normalized * (getTarget.GetComponent<MeshFilter>().mesh.bounds.size.z / 2) * getTarget.transform.localScale.z;
                        getTarget.transform.parent = hookChecker.transform;
                    }
                    else
                        getTarget.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case Item.TypeOfItem.Money:
                    if (canBeAttached)
                    {
                        //sumar cantidad a la caja
                        Destroy(getTarget);
                    }
                    else
                        getTarget.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                default:
                    break;
            }
        }
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (hit.collider.CompareTag("Draggable"))
                target = hit.collider.gameObject;
            else 
                target = null;
        }
        return target;
    }

    void SwitchSelector()
    {
        switch (typeOf._item)
        {
            case Item.TypeOfItem.Product:
                dadDelegate = CaseProduct;
                break;
            case Item.TypeOfItem.Money:
                dadDelegate = CaseMoney;
                break;
            default:
                break;
        }
        //if (hit.collider.CompareTag("Customer"))
        //{
        //    //funcion CompleteSale de la clase Customer.
        //}
    }

    void CaseProduct(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Attachment"))
        {
            hookChecker = hit.collider.GetComponent<HookChecker>();
            if (hookChecker.transform.childCount == 0)
            {
                getTarget.GetComponent<Renderer>().material.color = Color.green;
                attachmentTransform = hit.collider.transform;
                canBeAttached = true;

                Debug.DrawRay(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back) * hit.distance, Color.green);
            }
        }
        else
        {
            getTarget.GetComponent<Renderer>().material.color = Color.red;
            canBeAttached = false;

            Debug.DrawRay(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back) * hit.distance, Color.red);
        }
    }
    void CaseMoney(RaycastHit hit)
    {
        if (hit.collider.CompareTag("CashBox"))
        {
            getTarget.GetComponent<Renderer>().material.color = Color.green;
            attachmentTransform = hit.collider.transform;
            canBeAttached = true;

            Debug.DrawRay(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back) * hit.distance, Color.green);
        }
        else
        {
            getTarget.GetComponent<Renderer>().material.color = Color.red;
            canBeAttached = false;

            Debug.DrawRay(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back) * hit.distance, Color.red);
        }
    }
}
