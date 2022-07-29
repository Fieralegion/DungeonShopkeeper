using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public string itemName;
    public float price, sellPrice;
    public itemType _item;
    [HideInInspector]
    public string owner;
    [HideInInspector]
    public bool active;

    public AudioClip itemGrab;
    public AudioClip itemDrop;
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