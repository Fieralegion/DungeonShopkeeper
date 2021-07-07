using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumLib;
//arreglar opciones de juego
public class DialogueHandler : MonoBehaviour
{
    [SerializeField] float textboxLife, fadeSpeed;
    [SerializeField] int maxLength;
    [SerializeField] GameObject custText, option1, option2, option3, item;
    Image ctImage; 
    Image[] opImage;
    Text ctText; 
    Text[] opTextt;
    // Start is called before the first frame update
    // make it world dialogue
    void Start()
    {
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

    public void TraverseDialogueTree(DialogueNodes dn)
    {
        Debug.Log(dn.name);
        DeleteText();
        SummonText(dn.text, custText.transform.parent.GetComponent<Image>(), custText.GetComponent<Text>(), false);
        bool failed = true ;
        for (int i = 0; i < dn.nextNode.Length; i++)
        {
            if (dn.nextNode[i] && !dn.nextNode[i].activated && CheckConditional(dn.nextNode[i].c))
            {
                SummonText(dn.nextNode[i].text, opImage[i], opTextt[i], false);
                //Debug.Log(dn.nextNode[i].name + " : " + dn.nextNode[i].nextNode[0].name);
                int j = i;
                opImage[i].transform.GetComponent<Button>().onClick.AddListener(() => TraverseDialogueTree(dn.nextNode[j].nextNode[0]));
                opImage[i].transform.GetComponent<Button>().onClick.AddListener(() => dn.nextNode[j].activated = true);
                failed = false;
            }
        }
        if (failed)
        {
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<CustomerSpawner>().CompleteFirstCustom();
        }
    }

    bool CheckConditional(conditionals c)
    {
        switch (c)
        {
            case conditionals.None:
                return true;
            case conditionals.Item: //Check if item is in storage or store
                break;
            case conditionals.ItemInFront: //Check if item is in front
                break;
            case conditionals.Money: //Check if character has enough money
                break;
        }
        return false;
    }

    public void SummonText(string s, Image bubble, Text text, bool tb)
    {
        //text.text = s;
        text.text = TextAdjuster(s, null);
        bubble.color = Color.white;
        text.color = Color.black;
        if (tb)
        {
            StartCoroutine(AutoFade(bubble, text));
        }
    }
   /* public void SummonText(string s)
    {
        //text.text = s;
        ctText.text = TextAdjuster(s);
        ctImage.color = Color.white;
        ctText.color = Color.black;
        StartCoroutine(AutoFade(ctImage, ctText));
    }*/

    public void SummonText(string s, GameObject item)
    {
        //text.text = s;
        ctText.text = TextAdjuster(s, item.name);
        ctImage.color = Color.white;
        ctText.color = Color.black;
        StartCoroutine(AutoFade(ctImage, ctText));
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
            if (wpl == maxLength)
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
