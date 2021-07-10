using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags", menuName = "ScriptableObjects/Flags", order = 4)]
public class Flags : ScriptableObject
{
    [SerializeField] string[] tagNames;
    public Dictionary<string, bool> tagList;
    // Start is called before the first frame update

    public void InitializeFlags()
    {
        tagList = new Dictionary<string, bool>();
        foreach (string s in tagNames)
        {
            tagList.Add(s, false);
        }
        foreach (string s in tagList.Keys)
        {
            Debug.Log(s + " " + tagList[s]);
        }
    }

    public bool CheckTag(string name)
    {
        bool result;
        if (tagList.TryGetValue(name, out result))
        {
            Debug.Log(name + " " + tagList[name]);
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
            Debug.Log(name + " " + tagList[name]);
        }
    }
}
