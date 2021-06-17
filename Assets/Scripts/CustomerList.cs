using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class CustomerList : ScriptableObject
{
    public Customer[] customerList;

    [System.Serializable]
    public struct Customer
    {
        public GameObject customer;
        public float time;
    }
}
