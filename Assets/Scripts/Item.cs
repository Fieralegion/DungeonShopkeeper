using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public GameObject model;
    public float price;
    public itemType IT;
}
