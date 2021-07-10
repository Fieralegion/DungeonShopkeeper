using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumLib;
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] float textboxLife, fadeSpeed;
    [SerializeField] int maxLength;
    [SerializeField] GameObject custText, option1, option2, option3, item;
    [SerializeField] ItemList inventory;
    [SerializeField] Flags activeFlags;
    Image ctImage;
    Image[] opImage;
    Text ctText;
    Text[] opTextt;
    DialogueNodes[] nodes;
    bool canChoose;
    GameObject curCust;

    void Start()
    {
        activeFlags.InitializeFlags();
        nodes = new DialogueNodes[3];

        ctImage = custText.transform.parent.GetComponent<Image>();
        opImage = new Image[3];
        opImage[0] = option1.transform.parent.GetComponent<Image>();
        opImage[1] = option2.transform.parent.GetComponent<Image>();
        opImage[2] = option3.transform.parent.GetComponent<Image>();

        ctText = custText.GetComponent<Text>();
        opTextt = new Text[3];
        opTextt[0] = option1.GetComponent<Text>();
        opTextt[1] = option2.GetComponent<Text>();
        opTextt[2] = option3.GetComponent<Text>();
    }

    private void LateUpdate()
    {
        if (canChoose)
        {
            if (Input.GetButtonDown("Option1") && nodes[0])
            {
                canChoose = false;
                OptionPicker(0);
            }
            else if (Input.GetButtonDown("Option2") && nodes[1])
            {
                canChoose = false;
                OptionPicker(1);
            }
            else if (Input.GetButtonDown("Option3") && nodes[2])
            {
                canChoose = false;
                OptionPicker(2);
            }
        }
    }

    public void OptionPicker(int ind)
    {
        nodes[ind].activated = true;
        if (nodes[ind].r == resutls.None)
        {
            TraverseDialogueTree(nodes[ind].nextNode[0]);
        }
        else
        {
            DeleteText();
            ExecuteResult(nodes[ind]);
        }
    }

    public void OptionPicker(DialogueNodes dn)
    {
        dn.activated = true;
        if (dn.r == resutls.None)
        {
            TraverseDialogueTree(dn.nextNode[0]);
        }
        else
        {
            DeleteText();
            ExecuteResult(dn.nextNode[0]);
        }
    }

    IEnumerator OptionDelayed(DialogueNodes n)
    {
        yield return new WaitForSeconds(fadeSpeed);
        OptionPicker(n);
    }

    public void TraverseDialogueTree(DialogueNodes dn)
    {
        DeleteText();
        StopAllCoroutines();
        SummonText(dn.text, custText.transform.parent.GetComponent<Image>(), custText.GetComponent<Text>(), false);
        /*if (dn.r != resutls.None)
        {
            ExecuteResult(dn);
            return;
        }*/
        //else
        //{
            bool failed = true;
            foreach (DialogueNodes n in dn.nextNode)
            {
                if (CheckConditional(n))
                {
                    StartCoroutine(OptionDelayed(n));
                    return;
                }
            }
            for (int i = 0; i < dn.nextNode.Length; i++)
            {
                if (dn.nextNode[i] && !dn.nextNode[i].activated && dn.nextNode[i].c == conditionals.None && CheckAvailable(dn.nextNode[i]))
                {
                    int j = i;
                    nodes[j] = dn.nextNode[j];
                    failed = false;
                    canChoose = true;
                    SummonText(nodes[j].text, opImage[j], opTextt[j], false);
                }
        }
        if (dn.r != resutls.None)
        {
            ExecuteResult(dn);
        }
        if (failed)
            {
                curCust.GetComponent<Customer>().SetDestination(curCust.GetComponent<Customer>().finalDestination);
                GameObject.FindGameObjectWithTag("Respawn").GetComponent<CustomerSpawner>().MoveCustomer(curCust);
            }

        //}
    }

    public void TraverseDialogueTree(GameObject go)
    {
        curCust = go;
        DialogueNodes dn = go.GetComponent<Customer>().dialogue.firstNode;
        DeleteText();
        StopAllCoroutines();
        SummonText(dn.text, custText.transform.parent.GetComponent<Image>(), custText.GetComponent<Text>(), false);
        /*if (dn.r != resutls.None)
        {
            ExecuteResult(dn);
            return;
        }*/
        //else
        //{
            bool failed = true;
            foreach (DialogueNodes n in dn.nextNode)
            {
                if (CheckConditional(n))
                {
                    OptionPicker(n);
                    return;
                }
            }
            for (int i = 0; i < dn.nextNode.Length; i++)
            {
                if (dn.nextNode[i] && !dn.nextNode[i].activated && dn.nextNode[i].c == conditionals.None && CheckAvailable(dn.nextNode[i]))
                {
                    int j = i;
                    nodes[j] = dn.nextNode[j];
                    failed = false;
                    canChoose = true;
                    SummonText(nodes[j].text, opImage[j], opTextt[j], false);
                }
        }
        if (dn.r != resutls.None)
        {
            ExecuteResult(dn);
        }
        if (failed)
            {
                curCust.GetComponent<Customer>().SetDestination(curCust.GetComponent<Customer>().finalDestination);
                GameObject.FindGameObjectWithTag("Respawn").GetComponent<CustomerSpawner>().MoveCustomer(curCust);
            }
        //}
    }

    bool CheckAvailable(DialogueNodes node)
    {
        switch (node.a)
        {
            case available.Always:
                return true;
            case available.Flag:
                return activeFlags.CheckTag(node.flag);
            case available.Item:
                foreach (GameObject h in GameObject.FindGameObjectsWithTag("Attachment"))
                {
                    if (h.GetComponent<HookChecker>().actualItem == curCust.GetComponent<Customer>().itemBuy.GetComponent<Item>().itemName)
                    {
                        return true;
                    }
                }
                return inventory.SearchItem(curCust.GetComponent<Customer>().itemBuy);
            case available.Money:
                return node.value <= inventory.money;
        }
        return true;
    }

    bool CheckConditional(DialogueNodes node)
    {
        switch (node.c)
        {
            case conditionals.None:
                return false;
            case conditionals.Item:
                foreach (GameObject h in GameObject.FindGameObjectsWithTag("Attachment"))
                {
                    if (h.GetComponent<HookChecker>().actualItem == curCust.GetComponent<Customer>().itemBuy.GetComponent<Item>().itemName)
                    {
                        return true;
                    }
                }
                return inventory.SearchItem(curCust.GetComponent<Customer>().itemBuy);
            case conditionals.NoItem:
                foreach (GameObject h in GameObject.FindGameObjectsWithTag("Attachment"))
                {
                    if (h.GetComponent<HookChecker>().actualItem == curCust.GetComponent<Customer>().itemBuy.GetComponent<Item>().itemName)
                    {
                        return false;
                    }
                }
                return !inventory.SearchItem(curCust.GetComponent<Customer>().itemBuy);
            case conditionals.ItemInFront:
                foreach (GameObject h in GameObject.FindGameObjectsWithTag("Attachment"))
                {
                    if (h.GetComponent<HookChecker>().front&& h.GetComponent<HookChecker>().actualItem == curCust.GetComponent<Customer>().itemBuy.GetComponent<Item>().itemName)
                    {
                        return true;
                    }
                }
                return false;
            case conditionals.Money:
                return node.value <= inventory.money;
            case conditionals.Flag:
                return activeFlags.CheckTag(node.flag);
        }
        return false;
    }

    private void ExecuteResult(DialogueNodes node)
    {
        switch (node.r)
        {
            case resutls.BuySell:
                curCust.GetComponent<Customer>().SetActive();
                curCust.GetComponent<Customer>().nextDialogue = node;
                break;
            case resutls.Item:
                Instantiate(curCust.GetComponent<Customer>().itemSell, curCust.GetComponent<Customer>().bandeja, curCust.transform.rotation);
                break;
            case resutls.ActivateFlag:
                activeFlags.ModifyFlag(node.flag);
                break;
            case resutls.DeletePayment:
                curCust.GetComponent<Customer>().DestroyPayment();
                break;
            case resutls.SwapSell:
                GameObject temp = curCust.GetComponent<Customer>().itemSell;
                curCust.GetComponent<Customer>().itemSell = curCust.GetComponent<Customer>().money;
                curCust.GetComponent<Customer>().money = temp;
                break;
            case resutls.EditMoney:
                curCust.GetComponent<Customer>().itemSell.GetComponent<Item>().price = node.value;
                break;
        }
    }

    public void SummonText(string s, Image bubble, Text text, bool tb)
    {
        text.text = TextAdjuster(s, " ");
        bubble.color = Color.white;
        text.color = Color.black;
        if (tb)
        {
            StartCoroutine(AutoFade(bubble, text));
        }
    }

    public float SummonText(string s, GameObject item)
    {
        ctText.text = TextAdjuster(s, item.name);
        ctImage.color = Color.white;
        ctText.color = Color.black;
        StartCoroutine(AutoFade(ctImage, ctText));
        return textboxLife;
    }

    string TextAdjuster(string s, string name)
    {
        string temp = s.Replace("xxx", name);
        char[] compact = new char[temp.Length + temp.Length / maxLength];
        int wpl = 0, n = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            compact[i + n] = temp[i];
            wpl++;
            if (wpl >= maxLength && compact[i + n] == ' ')
            {
                wpl = 0;
                compact[i + n + 1] = '\n';
                n += 1;
            }
        }
        return new string(compact);
    }

    private void DeleteText()
    {
        Fading(ctImage, ctText);
        Fading(opImage[0], opTextt[0]);
        Fading(opImage[1], opTextt[1]);
        Fading(opImage[2], opTextt[2]);
    }

    IEnumerator AutoFade(Image bubble, Text text)
    {
        yield return new WaitForSeconds(textboxLife);
        Fading(bubble, text);
    }

    private void Fading(Image bubble, Text text)
    {
        while (bubble.color.a > 0)
        {
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, bubble.color.a - fadeSpeed * Time.deltaTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - fadeSpeed * Time.deltaTime);
        }
    }
}
