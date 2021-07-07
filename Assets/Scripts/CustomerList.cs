using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerList", menuName = "ScriptableObjects/CustomerList", order = 4)]
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
