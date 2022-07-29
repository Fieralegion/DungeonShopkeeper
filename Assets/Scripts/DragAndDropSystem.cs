using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumLib;
using GameManager;

public class DragAndDropSystem : MonoBehaviour
{
    private GameObject getTarget;
    private Transform attachmentTransform, customerTransform;
    private HookChecker hookChecker;
    private Item typeOfItem;

    [SerializeField] private Image cursorEmpty;
    [SerializeField] private Image cursorGrabbed;

    private bool isDragged;
    private bool canBeAttached, canBeSold;

    private delegate void DraggedDelegate(RaycastHit hit);
    private DraggedDelegate draggedDelegate;
    private delegate void EndDragDelegate();
    private EndDragDelegate endDragDelegate;

    [SerializeField] private AudioSource audioSource;


    void LateUpdate()
    {
        if (!GameSettings.gamePaused)
        {
            if (Input.GetMouseButtonDown(0))
                BeginDrag();

            if (isDragged)
                Dragged();

            if (Input.GetMouseButtonUp(0))
                EndDragg();
        }
    }
    void BeginDrag()
    {
        cursorEmpty.enabled = false;
        cursorGrabbed.enabled = true;
        getTarget = ReturnClickedObject(out RaycastHit hit);

        if (getTarget != null)
        {
            isDragged = true;
            getTarget.transform.parent = null;
            getTarget.GetComponent<Rigidbody>().isKinematic = true;
            getTarget.GetComponent<Rigidbody>().freezeRotation = true;
            typeOfItem = getTarget.GetComponent<Item>();
            if(typeOfItem.itemGrab != null) audioSource.PlayOneShot(typeOfItem.itemGrab);
            ItemSelector();
        }
    }
    void Dragged()
    {
        if (getTarget)
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.25f);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace);
            getTarget.transform.position = currentPosition;
            getTarget.transform.LookAt(Camera.main.transform.position);

            if (Physics.Raycast(getTarget.transform.position, getTarget.transform.TransformDirection(Vector3.back), out RaycastHit hit))
                draggedDelegate?.Invoke(hit);
        }
        else
        {
            EndDragg();
        }
    }
    void EndDragg()
    {
        isDragged = false;
    
        if (getTarget != null)
        {
            getTarget.GetComponent<Rigidbody>().freezeRotation = false;
            endDragDelegate?.Invoke();
            if (typeOfItem.itemDrop != null) audioSource.PlayOneShot(typeOfItem.itemDrop);
        }
        cursorEmpty.enabled = true;
        cursorGrabbed.enabled = false;
    }
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (hit.collider.CompareTag("Attachment"))
            {
                GameObject.FindGameObjectWithTag("Restock").GetComponent<RestockManager>().ActivateRestock(hit.collider.gameObject);
            }
            else
            {
                GameObject.FindGameObjectWithTag("Restock").GetComponent<RestockManager>().DeactivateRestock();
            }

            if (hit.collider.CompareTag("Draggable"))
            {
                target = hit.collider.gameObject;
                if (target.transform.parent != null)
                {
                    hookChecker = hit.transform.parent.GetComponent<HookChecker>();
                    hookChecker.previousItem = hit.collider.gameObject.GetComponent<Item>().itemName;
                    hookChecker.actualItem = "";
                    hookChecker = null;
                }
            }
            else if (hit.collider.CompareTag("CashBox"))
            {
                target = hit.collider.gameObject.GetComponent<MoneySystem>().SpawnMoney();
                target.GetComponent<Item>().active = true;
            }
            else 
                target = null;
        }
        return target;
    }
    void ItemSelector()
    {
        switch (typeOfItem._item)
        {
            case itemType.Money:
                draggedDelegate = CaseMoneyDragged;
                endDragDelegate = CaseMoneyEndDrag;
                break;
            default:
                draggedDelegate = CaseProductDragged;
                endDragDelegate = CaseProductEndDrag;
                break;
        }
    }
    void CaseProductDragged(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Attachment"))
        {
            hookChecker = hit.collider.GetComponent<HookChecker>();
            if (hookChecker.transform.childCount == 0)
            {
                attachmentTransform = hit.collider.transform;
                canBeAttached = true;
            }
        }
        else if (hit.collider.CompareTag("Customer"))
        {
            canBeSold = true;
            customerTransform = hit.transform;
        }
        else
        {
            canBeAttached = false;
            canBeSold = false;
        }
    }
    void CaseMoneyDragged(RaycastHit hit)
    {
        if (hit.collider.CompareTag("CashBox"))
        {
            attachmentTransform = hit.collider.transform;
            canBeAttached = true;
        }
        else if (hit.collider.CompareTag("Customer"))
        {
            canBeSold = true;
            customerTransform = hit.transform;
        }
        else
        {
            canBeAttached = false;
        }
    }

    void CaseProductEndDrag()
    {
        if (canBeAttached && hookChecker.transform.childCount == 0)
        {
            getTarget.transform.SetPositionAndRotation(attachmentTransform.position, attachmentTransform.rotation);
            getTarget.transform.position += attachmentTransform.forward.normalized * (getTarget.GetComponent<MeshFilter>().mesh.bounds.size.z / 2) * getTarget.transform.localScale.z;
            getTarget.transform.parent = hookChecker.transform;
            hookChecker.actualItem = getTarget.GetComponent<Item>().itemName;
        }
        else if (canBeSold)
        {
            if (!customerTransform.GetComponent<Customer>().CompleteSale(getTarget))
            {
                canBeSold = false;
            }
            else
            {
                getTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
            getTarget.GetComponent<Rigidbody>().isKinematic = false;
    }
    void CaseMoneyEndDrag()
    {
        if (canBeAttached && getTarget.GetComponent<Item>().active)
        {
            GameObject.FindGameObjectWithTag("CashBox").GetComponent<MoneySystem>().ModifyMoney(getTarget.GetComponent<Item>().price);
            Destroy(getTarget);
        }
        else if (canBeSold)
        {
            if (!customerTransform.GetComponent<Customer>().CompleteSale(getTarget))
            {
                getTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                getTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
            getTarget.GetComponent<Rigidbody>().isKinematic = false;
    }
}