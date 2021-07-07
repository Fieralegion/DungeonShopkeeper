using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 5)]
public class ItemList : ScriptableObject
{
    //Como chequeas los objetos colgando
    public GameObject[] itemStorage;

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
            if (i.name == itemStorage[j].name && n == 0)
            {
                n++;
            }
            else
            {
                tempStorage[j - n] = itemStorage[j];
            }
        }

        itemStorage = tempStorage;
    }

    public bool SearchItem(GameObject i)
    {
        foreach (GameObject t in itemStorage)
        {
            if (t.name == i.name) return true;
        }
        return false;
    }
}
