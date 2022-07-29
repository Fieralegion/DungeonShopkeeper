using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags", menuName = "ScriptableObjects/Flags", order = 4)]
public class Flags : ScriptableObject
{
    [SerializeField] string[] tagNames;
    public Dictionary<string, bool> tagList;
    public void InitializeFlags()
    {
        tagList = new Dictionary<string, bool>();
        foreach (string s in tagNames)
        {
            tagList.Add(s, false);
        }
    }

    public bool CheckTag(string name)
    {
        bool result;
        if (tagList.TryGetValue(name, out result))
        {
            return tagList[name];
        }
        else
        {
            return false;
        }

    }

    public void ModifyFlag(string name)
    {
        bool result;
        if (tagList.TryGetValue(name, out result))
        {
            tagList[name] = true;
        }
    }
}
