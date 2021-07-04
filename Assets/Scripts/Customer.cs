using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumLib;
[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    public enum custType { Casual, Core, Hardcore };
    public custType CT;
    public Dialogue dialogue;
     public DialogueNodes nextDialogue;
    [SerializeField] float leaveTime;
    [SerializeField] GameObject money;
    public GameObject itemBuy, itemSell, otherItem;
    DialogueHandler textHandler;
    [SerializeField] bool buy, sell;
    protected bool active;
    bool wrong;
    public Vector3 finalDestination;
    NavMeshAgent agent;

    GameObject payment;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        textHandler = GameObject.FindGameObjectWithTag("DialogueHandler").GetComponent<DialogueHandler>();
        if (CT != custType.Hardcore)
        {
            StartCoroutine(Leave());
            if (CT == custType.Casual)
            {
                int i = Random.Range(1, 100);
                if (i > 0 && i <= 60)
                {
                    buy = true;
                }
                else if (i > 60 && i <= 90)
                {
                    sell = true;
                }
                else
                {
                    buy = true;
                    sell = true;
                }
            }
        }
    }

    /* IEnumerator TurnAround()
     {
         yield return new WaitForSeconds(3f);
         Debug.Log("Rotated");
         transform.Rotate(Vector3.Lerp(transform.forward, Vector3.right, 0.5f));
     }*/

    // Update is called once per frame
    void Update()
    {

    }

    public bool CompleteSale(GameObject item)
    {
        if (active)
        {
            if (buy)
            {
                if (item.GetComponent<Item>().itemName == itemBuy.GetComponent<Item>().itemName)
                {
                    if (!sell)
                    {
                        FinishTransaction(true);
                        payment.GetComponent<Item>().owner = "Shopowner";
                        payment.GetComponent<Item>().active = true;
                    }
                    else
                    {
                        textHandler.SummonText(dialogue.RandomDialogue(textType.Sell), itemSell);
                    }
                    Destroy(item);
                    return true;
                }
                else if (otherItem && item.GetComponent<Item>().itemName == otherItem.GetComponent<Item>().itemName)
                {
                    if (!sell)
                    {
                        FinishTransaction(true);
                        payment.GetComponent<Item>().owner = "Shopowner";
                        payment.GetComponent<Item>().active = true;
                    }
                    else
                    {
                        textHandler.SummonText(dialogue.RandomDialogue(textType.Sell), itemSell);
                    }
                    wrong = true;
                    Destroy(item);
                    return true;
                }
                else if (item.GetComponent<Item>()._item == itemType.Money && item.GetComponent<Item>().owner == "Customer")
                {
                    FinishTransaction(false);
                    Destroy(payment);
                }
            }

            if (sell && item.GetComponent<Item>()._item == itemType.Money) //chequear la cantidad de dinero restante. Chequear si es un objeto de dinero
            {

                if (item.GetComponent<Item>().price == 0)
                {
                    GameObject cash = GameObject.FindGameObjectWithTag("CashBox");
                    if (cash.GetComponent<MoneySystem>().CheckMoney() >= itemSell.GetComponent<Item>().price)
                    {
                        FinishTransaction(true);
                        cash.GetComponent<MoneySystem>().ModifyMoney(-itemSell.GetComponent<Item>().price);
                        payment.GetComponent<Item>().owner = "Shopowner";
                        Destroy(item);
                        return true;
                    }
                    else
                    {
                        FinishTransaction(false);
                        return false;
                    }
                }
                else if (item.GetComponent<Item>().price >= itemSell.GetComponent<Item>().price)
                {
                    FinishTransaction(true);
                    item.GetComponent<Item>().price -= itemSell.GetComponent<Item>().price;
                    payment.GetComponent<Item>().owner = "Shopowner";
                    return true;
                }
            }
            else if (sell && item.GetComponent<Item>()._item != itemType.Money && item.GetComponent<Item>().owner == "Customer")
            {
                FinishTransaction(false);
                Destroy(payment);
            }
        }
        if (CT != custType.Casual)
        {
            FinishTransaction(false);
            Destroy(payment);
        }
        return false;
    }

    void FinishTransaction(bool good)
    {
        StopAllCoroutines();
        if (CT == custType.Casual)
        {
            if (good)
            {
                textHandler.SummonText(dialogue.RandomDialogue(textType.DepartHappy), itemBuy);
            }
            else
            {
                textHandler.SummonText(dialogue.RandomDialogue(textType.DepartAngry), itemBuy);
            }
            SetDestination(finalDestination);
        }
        else
        {
            if (good)
            {
                if (!wrong)
                {
                    textHandler.TraverseDialogueTree(nextDialogue.nextNode[0]);
                }
                else
                {
                    textHandler.TraverseDialogueTree(nextDialogue.nextNode[2]);
                }
            }
            else
            {
                textHandler.TraverseDialogueTree(nextDialogue.nextNode[1]);
            }
        }
    }

    public void SetDestination(Vector3 pos)
    {
        agent.destination = pos;
    }

    public void SetFinalDestination(Vector3 v)
    {
        finalDestination = v;
    }


    IEnumerator Leave()
    {
        yield return new WaitForSeconds(leaveTime);
        active = true;
        SetDestination(finalDestination);
    }

    IEnumerator StartBusiness()
    {
        yield return new WaitForSeconds(textHandler.SummonText(dialogue.RandomDialogue(textType.Greeting), itemBuy));
        if (buy)
        {
            textHandler.SummonText(dialogue.RandomDialogue(textType.Buy), itemBuy);
        }
        else
        {
            textHandler.SummonText(dialogue.RandomDialogue(textType.Sell), itemSell);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shop")
        {
            active = true;
            if (buy)
            {
                payment = Instantiate(money, transform.position + Vector3.right * 2, transform.rotation);
                payment.GetComponent<Item>().price = itemBuy.GetComponent<Item>().price;
                payment.GetComponent<Item>().owner = "Customer";
            }
            else
            {
                payment = Instantiate(itemSell, transform.position + Vector3.right * 2, transform.rotation);
                payment.GetComponent<Item>().owner = "Customer";
            }

            if (CT == custType.Casual)
            {
                StartCoroutine(StartBusiness());
            }
            else
            {
                textHandler.TraverseDialogueTree(gameObject);
                if (CT == custType.Hardcore)
                {
                    //Time.timeScale = 0;
                }
            }
        }
    }

    public void SetActive()
    {
        active = true;
    }

    public bool IsActive()
    {
        return active;
    }
}
