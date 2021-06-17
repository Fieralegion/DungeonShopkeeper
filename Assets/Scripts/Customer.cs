using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumLib;
[RequireComponent (typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    public bool sale = false;

    public enum custType { Casual, Core, Hardcore };
    public custType CT;
    [SerializeField] Dialogue dialogue;
    [SerializeField] float leaveTime;
    DialogueHandler textHandler;
    bool active;
    Vector3 finalDestination;
    NavMeshAgent agent;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        textHandler = GameObject.FindGameObjectWithTag("DialogueHandler").GetComponent<DialogueHandler>();
        if (CT != custType.Hardcore)
        {
            StartCoroutine(Leave());
        }
        else
        {
            Time.timeScale = 0;
        }
        //StartCoroutine(TurnAround()); fix later
    }

    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Rotated");
        transform.Rotate(Vector3.Lerp(transform.forward, Vector3.right, 0.5f));
    }

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
            textHandler.SummonText(dialogue.RandomDialogue(t));
        }
        else
        {
            textHandler.TraverseDialogueTree(dialogue.firstNode);
        }
    }

    public void SaleEnd()
    {

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
