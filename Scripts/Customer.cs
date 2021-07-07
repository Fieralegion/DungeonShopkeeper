using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumLib;
[RequireComponent (typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    //trabajar modo de venta
    public bool sale = false;

    public enum custType { Casual, Core, Hardcore };
    public custType CT;
    [SerializeField] Dialogue dialogue;
    [SerializeField] float leaveTime;
    public GameObject itemBuy, itemSell;
    DialogueHandler textHandler;
    bool active, buy, sell;
    Vector3 finalDestination;
    NavMeshAgent agent;

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
        else
        {
            Time.timeScale = 0;
        }
        //StartCoroutine(TurnAround()); fix later
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
        if (sale)
        {
            SetDestination(finalDestination);
            if (CT == custType.Hardcore)
            {
                Time.timeScale = 1;
            }
        }
        if (agent.remainingDistance <= 1 && active)
        {
            PlayDialogue(textType.Greeting);
            active = false;
        }
    }

    void PlayDialogue(textType t)
    {
        if (CT == custType.Casual)
        {
            textHandler.SummonText(dialogue.RandomDialogue(t), itemBuy);

        }
        else
        {
            textHandler.TraverseDialogueTree(dialogue.firstNode);
        }
    }

    public bool CompleteSale(GameObject item)
    {
        if (buy && item.name == itemBuy.name)
        {
            //spawnear el dinero
            if (!sell) { FinishTransaction();
                Destroy(item);
            }
            return true;
        }
        else if (item.name != itemBuy.name) //Que pasa al objeto si no es el correcto
        {
            Debug.Log("Wrong item!");
            return false;
        }

        if (sell && item.GetComponent<Item>()._item == itemType.Money) //chequear la cantidad de dinero restante. Chequear si es un objeto de dinero
        {
            //das dinero
            FinishTransaction();
            return true;
        }
        else if (item.name != itemBuy.name) //Que pasa si le das otro objeto/no tienes dinero
        {
            return false;
        }

        return false;
    }

    void FinishTransaction()
    {
        sale = true;
        SetDestination(finalDestination);
        if (CT == custType.Hardcore)
        {
            Time.timeScale = 1;
        }
    }

    public void SetDestination(Vector3 pos)
    {
        agent.destination = pos;
    }

    public void SetActive()
    {
        active = true;
    }

    public void SetFinalDestination(Vector3 v)
    {
        finalDestination = v;
    }
    

     IEnumerator Leave()
    {
        yield return new  WaitForSeconds(leaveTime);
        sale = true;
        SetDestination(finalDestination);
    }
}
