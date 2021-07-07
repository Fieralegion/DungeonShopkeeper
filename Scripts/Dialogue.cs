using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    public string[] greetingsD, goodbyeD, thankyouD, fuD;
    public DialogueNodes firstNode;
    public GameObject item;

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
