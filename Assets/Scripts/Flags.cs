using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags", menuName = "ScriptableObjects/Flags", order = 4)]
public class Flags : MonoBehaviour
{
    [SerializeField] string[] tagNames;
    public Dictionary<string, bool> tagList;
    // Start is called before the first frame update
    
    void Start()
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
        tagList.TryGetValue(name, out result);
        return result;
    }

    public void ModifyFlag(string name)
    {
        if (tagList[name])
        {
            tagList[name] = true;
        }
    }
}
