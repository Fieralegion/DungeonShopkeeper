using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//implementar giro hacia adelante
//core y hardcore salen a ciertos minutos especificos
public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] Vector2 minmaxCooldown;
    [SerializeField] int maxCustomers;
    [SerializeField] GameObject spawner1, spawner2, stopPoint;
    [SerializeField] GameObject[] customers;
    [SerializeField] CustomerList[] importantCustomers;
    [SerializeField] GameObject[] purchaseAvailable;

    [System.Serializable]
    public struct CustomerList
    {
        public GameObject customer;
        public float time;
    }

    List<GameObject> customerList;
    GlobalTimer gt;
    int impIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        customerList = new List<GameObject>();
        gt = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<GlobalTimer>();
        StartCoroutine(TimedSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimedSpawn()
    {
        yield return new WaitForSeconds(Random.Range(minmaxCooldown.x, minmaxCooldown.y));
        GameObject cust;
        if (impIndex < importantCustomers.Length && gt.currentTime > importantCustomers[impIndex].time * 60)
        {
            cust = importantCustomers[impIndex].customer;
            impIndex++;
        }
        else
        {
            cust = customers[Random.Range(0, customers.Length - 1)];
        }
        GameObject go = Instantiate(cust, RandomPoint(), transform.rotation);
        go.GetComponent<Customer>().SetDestination(stopPoint.transform.position + customerList.Count * Vector3.left * 2);
        go.GetComponent<Customer>().SetFinalDestination(RandomPoint());
        if (go.GetComponent<Customer>().CT == Customer.custType.Casual)
        {
            if (Random.Range(0, 100) <= 65)
            {
                go.GetComponent<Customer>().itemBuy = purchaseAvailable[0]; //Temp. debe ser Item del mostrador. Hay que cambiar
                go.GetComponent<Customer>().itemSell = purchaseAvailable[Random.Range(0, purchaseAvailable.Length)];
            }
            else
            {
                go.GetComponent<Customer>().itemBuy = purchaseAvailable[Random.Range(0, purchaseAvailable.Length)];
                go.GetComponent<Customer>().itemSell = purchaseAvailable[Random.Range(0, purchaseAvailable.Length)];
            }
        }
        if (customerList.Count == 0)
        {
            go.GetComponent<Customer>().SetActive();
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

    public void CompleteFirstCustom()
    {
        customerList[0].GetComponent<Customer>().sale = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Customer>().sale)
        {
            customerList.Remove(other.gameObject);
            float mult = 0;
            foreach(GameObject g in customerList)
            {
                g.GetComponent<Customer>().SetDestination(stopPoint.transform.position + mult * Vector3.left * 2);
                mult++;
            }
            if (customerList.Count != 0)
            {
                customerList[0].GetComponent<Customer>().SetActive();
            }
            Destroy(other.gameObject);
        }
    }
}
