using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public GameObject model; //no necesario?
    public float price;
    public enum TypeOfItem { Product, Money}
    public TypeOfItem _item;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    Item targetItem;
    private void OnEnable()
    {
        targetItem = (Item)target;
        targetItem.tag = "Draggable";
        targetItem.GetComponent<Rigidbody>().hideFlags = HideFlags.HideInInspector;
    }
}
#endif