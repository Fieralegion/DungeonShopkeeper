using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public Slider volSlider;
    [SerializeField] public Button startButton;
    [SerializeField] public Button exitButton;
    [SerializeField] public Dropdown resDrop;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject menuPanel;

    public Resolution[] resolutions;

    [SerializeField] private SaveConfig saveConfig;

    private void OnEnable()
    {
        SetResolutionOnDropdown();
        saveConfig.LoadSettings();
    }
    private void Awake()
    {
        resolutions = Screen.resolutions;
        startButton.onClick.AddListener(() => GameSettings.ChangeScene(1));
        startButton.onClick.AddListener(() => GameSettings.DisableMouse());
        volSlider.onValueChanged.AddListener(GameSettings.ChangeVolume);
        resDrop.onValueChanged.AddListener(GameSettings.ChangeResolution);
        exitButton.onClick.AddListener(GameSettings.CloseGame);
    }
    private void Update()
    {
        if (Input.anyKey) GameSettings.PushAnyKey(mainPanel, menuPanel);
    }
    private void SetResolutionOnDropdown()
    {
        resDrop.ClearOptions();
        
        int currentResIndex = 0;
        List<string> resList = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resList.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResIndex = i;
        }

        resDrop.AddOptions(resList);
        resDrop.value = currentResIndex;
        resDrop.RefreshShownValue();
    }
}