using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestockManager : MonoBehaviour
{
    [SerializeField] GameObject autoRestock, invRestock, selectSquare;
    [SerializeField] ItemList availableObjects, inventory;
    public bool active, activeMenu;
    GameObject curSlot, inventoryMenu;
    Vector3 initPos;
    int index;
    void Start()
    {
        inventoryMenu = GameObject.FindGameObjectWithTag("Inventory");
        initPos = selectSquare.GetComponent<RectTransform>().localPosition;
    }

    void Update()
    {
        if (active && curSlot.GetComponent<HookChecker>().actualItem == "")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (inventory.SearchItem(curSlot.GetComponent<HookChecker>().previousItem))
                {
                    GameObject g = Instantiate(inventory.SpawnItem(curSlot.GetComponent<HookChecker>().previousItem), curSlot.transform.position, transform.rotation);
                    inventory.RemoveItem(g);
                    inventoryMenu.GetComponent<InventoryBook>().UpdateInventoryText();
                }
                DeactivateRestock();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                selectSquare.GetComponent<RectTransform>().localPosition = initPos;
                index = 0;
                active = false;
                ToggleInventory(true);
                autoRestock.SetActive(false);
                invRestock.SetActive(false);
            }
        }

        if (activeMenu)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && index > 0)
            {
                selectSquare.GetComponent<RectTransform>().localPosition += new Vector3(0, 25, 0);
                index--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && index < availableObjects.itemStorage.Length - 1)
            {
                index++;
                selectSquare.GetComponent<RectTransform>().localPosition -= new Vector3(0, 25, 0);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (inventory.SearchItem(availableObjects.itemStorage[index]))
                {
                    GameObject g = Instantiate(availableObjects.itemStorage[index], curSlot.transform.position, transform.rotation);
                    inventory.RemoveItem(g);
                    inventoryMenu.GetComponent<InventoryBook>().UpdateInventoryText();
                }
                ToggleInventory(false);
                curSlot = null;
            }
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                ToggleInventory(false);
                curSlot = null;
            }
        }
    }

    public void ToggleInventory(bool bul)
    {
        activeMenu = bul;
        inventoryMenu.GetComponent<InventoryBook>().menuActive = bul;
        selectSquare.SetActive(bul);
    }

    public void ActivateRestock(GameObject slot)
    {
        curSlot = slot;
        active = true;
        autoRestock.SetActive(true);
        invRestock.SetActive(true);
    }

    public void DeactivateRestock()
    {
        active = false;
        autoRestock.SetActive(false);
        invRestock.SetActive(false);
        curSlot = null;
    }
}
