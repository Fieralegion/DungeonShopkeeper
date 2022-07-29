using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] Vector2 minmaxCooldown;
    [SerializeField] int maxCustomers;
    [SerializeField] float lineTime;
    [SerializeField] GameObject spawner1, spawner2, stopPoint;
    [SerializeField] GameObject[] customers;
    [SerializeField] CustomerList[] importantCustomers;
    [SerializeField] ItemList purchaseAvailable;
    [SerializeField] Flags flag;

    [System.Serializable]
    public struct CustomerList
    {
        public GameObject customer;
        public float time;
    }

    List<GameObject> customerList;
    GlobalTimer gt;
    int impIndex = 0;

    void Start()
    {
        customerList = new List<GameObject>();
        gt = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<GlobalTimer>();
        StartCoroutine(TimedSpawn());
    }
    IEnumerator TimedSpawn()
    {
        yield return new WaitForSeconds(Random.Range(minmaxCooldown.x, minmaxCooldown.y));
        GameObject cust;
        if (impIndex < importantCustomers.Length && gt.currentTime > importantCustomers[impIndex].time * 60)
        {
            if (importantCustomers[impIndex].customer.GetComponent<Customer>().dialogue.firstNode.a == EnumLib.available.Always || flag.CheckTag(importantCustomers[impIndex].customer.GetComponent<Customer>().dialogue.firstNode.flag))
            {
                cust = importantCustomers[impIndex].customer;
            }
            else
            {
                cust = customers[Random.Range(0, customers.Length)];
            }
            impIndex++;
        }
        else
        {
            cust = customers[Random.Range(0, customers.Length)];
        }
        GameObject go = Instantiate(cust, RandomPoint(), transform.rotation);
        go.GetComponent<Customer>().SetDestination(stopPoint.transform.position + customerList.Count * Vector3.left * 2);
        go.GetComponent<Customer>().SetFinalDestination(RandomPoint());
        if (go.GetComponent<Customer>().CT == Customer.custType.Casual)
        {
            List<GameObject> front = new List<GameObject>();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Attachment"))
            {
                if (g.GetComponent<HookChecker>().front && g.GetComponent<HookChecker>().actualItem != "")
                {
                    front.Add(purchaseAvailable.SpawnItem(g.GetComponent<HookChecker>().actualItem));
                }
            }
            if (Random.Range(0, 100) <= 65 && front.Count >= 1)
            {
                go.GetComponent<Customer>().itemBuy = purchaseAvailable.itemStorage[Random.Range(0, front.Count)];

            }
            else
            {
                go.GetComponent<Customer>().itemBuy = purchaseAvailable.itemStorage[Random.Range(0, purchaseAvailable.itemStorage.Length)];
            }
            while (go.GetComponent<Customer>().itemSell == null || go.GetComponent<Customer>().itemSell == go.GetComponent<Customer>().itemBuy)
            {
                go.GetComponent<Customer>().itemSell = purchaseAvailable.itemStorage[Random.Range(0, purchaseAvailable.itemStorage.Length)];
            }
        }
        customerList.Add(go);
        if (customerList.Count < maxCustomers)
        {
            StartCoroutine(TimedSpawn());
        }
        else
        {
            StartCoroutine(WaitForEnemies());
        }
    }

    IEnumerator WaitForEnemies()
    {
        yield return new WaitForSeconds(15);
        if (customerList.Count < maxCustomers)
        {
            StartCoroutine(TimedSpawn());
        }
        else
        {
            StartCoroutine(WaitForEnemies());
        }
    }

    Vector3 RandomPoint()
    {
        int n = Random.Range(0, 100);
        if (n <= 50)
        {
            return spawner1.transform.position;
        }
        else
        {
            return spawner2.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Customer>().IsActive())
        {
            Destroy(other.gameObject);
        }
    }

    public void MoveCustomer(GameObject cust)
    {
        StartCoroutine(LineDelay(cust));
    }

    IEnumerator LineDelay(GameObject cust)
    {
        yield return new WaitForSeconds(lineTime);
        cust.GetComponentInChildren<Animator>().SetBool("isWalking", true);
        customerList.Remove(cust);
        float mult = 0;
        foreach (GameObject g in customerList)
        {
            g.GetComponent<Customer>().SetDestination(stopPoint.transform.position + mult * Vector3.left * 2);
            mult++;
        }
    }
}