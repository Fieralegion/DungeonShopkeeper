using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] float textboxLife, fadeSpeed;
    [SerializeField] int maxLength;
    [SerializeField] GameObject custText, option1, option2, option3;
    Image ctImage; 
    Image[] opImage;
    Text ctText; 
    Text[] opTextt;
    // Start is called before the first frame update
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
            if (dn.nextNode[i] && !dn.nextNode[i].activated)
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

        //disappear buttons
        //spawn text
        //spawn buttons
        //each button spawns new traverse
    }

    public void SummonText(string s, Image bubble, Text text, bool tb)
    {
        //text.text = s;
        text.text = TextAdjuster(s);
        bubble.color = Color.white;
        text.color = Color.black;
        if (tb)
        {
            StartCoroutine(AutoFade(bubble, text));
        }
    }
    public void SummonText(string s)
    {
        //text.text = s;
        ctText.text = TextAdjuster(s);
        ctImage.color = Color.white;
        ctText.color = Color.black;
        StartCoroutine(AutoFade(ctImage, ctText));
    }

    string TextAdjuster(string s)
    {
        char[] compact = new char[s.Length + s.Length / maxLength];
        int wpl = 0, n = 0;
        for (int i = 0; i < s.Length; i++)
        {
            compact[i + n] = s[i];
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
