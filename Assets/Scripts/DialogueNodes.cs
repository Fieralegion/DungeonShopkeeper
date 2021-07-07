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
    public GameObject item;
    public float value;
    public resutls r;
    public conditionals c;
    public string flag;

    private void OnEnable()
    {
        activated = false;
    }

    private void Awake()
    {
        activated = false;
    }
}
