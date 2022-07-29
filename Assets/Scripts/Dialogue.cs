using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    public string[] greetingsD, buy, sell, happy, angry;
    public DialogueNodes firstNode;

    public string RandomDialogue(textType t)
    {
        switch (t)
        {
            case textType.Greeting:
                return greetingsD[Random.Range(0, greetingsD.Length)];
            case textType.Buy:
                return buy[Random.Range(0, buy.Length)];
            case textType.Sell:
                return sell[Random.Range(0, sell.Length)];
            case textType.DepartHappy:
                return happy[Random.Range(0, happy.Length)];
            case textType.DepartAngry:
                return angry[Random.Range(0, angry.Length)];
        }
        return null;
    }
}
