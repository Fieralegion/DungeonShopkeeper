using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] ItemList inventory;
    [SerializeField] GameObject money;

    float maxBags = 2, currentBags = 0;

    public void ModifyMoney(float quantity)
    {
        inventory.money += quantity;
        currentBags = Mathf.Max(0, currentBags - 1);
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryBook>().UpdateMoney();
    }

    public GameObject SpawnMoney()
    {
        if (currentBags < maxBags)
        {
            currentBags++;
            return Instantiate(money, transform.position, transform.rotation);
        }
        else
        {
            return null;
        }
    }

    public float CheckMoney()
    {
        return inventory.money;
    }
}
