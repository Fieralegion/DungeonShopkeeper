using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 5)]
public class ItemList : ScriptableObject
{
    public GameObject[] itemStorage;
    public float money;

    public void AddItem(GameObject i)
    {
        GameObject[] newStorage = new GameObject[itemStorage.Length + 1];
        newStorage = itemStorage;
        newStorage[newStorage.Length] = i;
        itemStorage = newStorage;
    }

    public void RemoveItem(GameObject i)
    {
        GameObject[] tempStorage = new GameObject[itemStorage.Length - 1];
        int n = 0;
        for (int j = 0; j < tempStorage.Length; j++)
        {
            
            if (i.GetComponent<Item>().itemName == itemStorage[j].GetComponent<Item>().itemName && n == 0)
            {
                n = n + 1;
            }
            tempStorage[j] = itemStorage[j + n];
        }

        itemStorage = tempStorage;
    }

    public bool SearchItem(GameObject i)
    {
        foreach (GameObject t in itemStorage)
        {
            if (t.GetComponent<Item>().itemName == i.GetComponent<Item>().itemName) return true;
        }
        return false;
    }

    public bool SearchItem(string i)
    {
        foreach (GameObject t in itemStorage)
        {
            if (t.GetComponent<Item>().itemName == i) return true;
        }
        return false;
    }

   public GameObject SpawnItem(string name)
    {
        foreach (GameObject t in itemStorage)
        {
            if (t.GetComponent<Item>().itemName == name) return t;
        }
        return null;
    }

    public void Reset(ItemList it)
    {
        itemStorage = new GameObject[it.itemStorage.Length];
        for (int i = 0; i < it.itemStorage.Length; i++)
        {
            itemStorage[i] = it.itemStorage[i];
        }
        money = it.money;
    }
}
