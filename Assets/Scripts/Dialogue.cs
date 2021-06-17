using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;//ask about dialogues

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    public string[] greetingsD, goodbyeD, thankyouD, fuD;
    public int interactionN;
    public DialogueNodes firstNode;

    public string OrderedDialogue(textType t)
    {
        string temp = null;
        switch (t)
        {
            case textType.Greeting:
                temp = greetingsD[interactionN];
                break;
        }
        interactionN++;
        return temp;
    }

    public string RandomDialogue(textType t)
    {
        switch (t)
        {
            case textType.Greeting:
                return greetingsD[Random.Range(0, greetingsD.Length)];
        }
        return null;
    }


}
