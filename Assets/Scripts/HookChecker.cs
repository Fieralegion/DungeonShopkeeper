using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookChecker : MonoBehaviour
{
    public bool canUse;
    private void Awake()
    {
        canUse = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Draggable"))
        {
            canUse = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Draggable"))
        {
            canUse = true;
        }
    }
}
