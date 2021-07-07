using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

[CreateAssetMenu(fileName = "SaveConfig", menuName = "ScriptableObjects/SaveConfig", order = 1)]
public class SaveConfig : ScriptableObject
{

    public void SaveSettings()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        PlayerPrefs.SetFloat("Volume", menuManager.volSlider.value);
        PlayerPrefs.SetInt("ResolutionIndex", menuManager.resDrop.value);
    }

    public void LoadSettings()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        menuManager.volSlider.value = PlayerPrefs.GetFloat("Volume");
        menuManager.resDrop.value = PlayerPrefs.GetInt("ResolutionIndex");
    }
}
