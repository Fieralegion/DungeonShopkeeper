using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryBook : MonoBehaviour
{
    [SerializeField] float maxHeight, speed, itemsPerList;
    [SerializeField] ItemList availableItems, inventory;
    [SerializeField] GameObject nameList1, nameList2, quantList1, quantList2, money;
    RectTransform obj;
    Vector3 basePos, finalPos;
    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<RectTransform>();
        basePos = obj.localPosition;
        finalPos = basePos + new Vector3(0, maxHeight);
        InitializeNames();
        UpdateInventoryText();
        UpdateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Inv"))
        {
            if (obj.localPosition.y < finalPos.y)
            {
                obj.localPosition += new Vector3(0, speed * Time.deltaTime);
            }
        }
        else
        {
            if (obj.localPosition.y > basePos.y)
            {
                obj.localPosition -= new Vector3(0, speed * Time.deltaTime);
            }
        }
    }

    private void InitializeNames()
    {
        string list1 = "";
        string list2 = "";
        for (int i = 0; i < availableItems.itemStorage.Length; i++)
        {
            if (i < itemsPerList)
            {
                list1 += availableItems.itemStorage[i].name + '\n';
            }
            else
            {
                list2 += availableItems.itemStorage[i].name + '\n';
            }
        }
        nameList1.GetComponent<Text>().text = list1;
        nameList2.GetComponent<Text>().text = list2;
    }

    public void UpdateInventoryText()
    {
        string list1 = "";
        string list2 = "";
        for (int i = 0; i < availableItems.itemStorage.Length; i++)
        {
            int quant = 0;
            foreach (GameObject item in inventory.itemStorage)
            {
                if (item.name == availableItems.itemStorage[i].name) quant++;
            }
            if (i < itemsPerList)
            {
                list1 += quant.ToString() + '\n';
            }
            else
            {
                list2 += quant.ToString() + '\n';
            }
        }
        quantList1.GetComponent<Text>().text = list1;
        quantList2.GetComponent<Text>().text = list2;
    }

    public void UpdateMoney()
    {
        money.GetComponent<Text>().text = inventory.money.ToString();
    }
}
