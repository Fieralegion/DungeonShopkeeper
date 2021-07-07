using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumLib;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "ScriptableObjects/DialogueNode", order = 3)]
public class DialogueNodes : ScriptableObject
{
    public string text;
    public DialogueNodes[] nextNode;
    public bool activated;
    public resutls r;
    public conditionals c;

    private void OnEnable()
    {
        activated = false;
    }

    private void Awake()
    {
        activated = false;
    }
}
