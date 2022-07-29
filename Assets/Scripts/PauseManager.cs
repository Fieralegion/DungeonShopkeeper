using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;

public class PauseManager : MonoBehaviour
{
    [SerializeField] public Slider volSlider;
    [SerializeField] public Button continueButton;
    [SerializeField] public Button mainMenuButton;
    [SerializeField] public Dropdown resDrop;
    [SerializeField] private GameObject pausePanel;

    public Resolution[] resolutions;

    [SerializeField] private SaveConfig saveConfig;

    private void OnEnable()
    {
        SetResolutionOnDropdown();
        saveConfig.LoadSettingsOnPauseMenu();
    }
    private void Awake()
    {
        resolutions = Screen.resolutions;
        continueButton.onClick.AddListener(() => GameSettings.gamePaused = false);
        continueButton.onClick.AddListener(() => GameSettings.PauseGame());
        continueButton.onClick.AddListener(() => GameSettings.DisableMouse());
        continueButton.onClick.AddListener(() => pausePanel.SetActive(false));
        volSlider.onValueChanged.AddListener(GameSettings.ChangeVolume);
        resDrop.onValueChanged.AddListener(GameSettings.ChangeResolution);
        mainMenuButton.onClick.AddListener(() => GameSettings.EnableMouse());
        mainMenuButton.onClick.AddListener(() => GameObject.FindWithTag("GlobalTimer").GetComponent<GlobalTimer>().ResetInventory());
        mainMenuButton.onClick.AddListener(() => GameSettings.ChangeScene(0));
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pausePanel.SetActive(true);
            GameSettings.gamePaused = true;
            GameSettings.PauseGame();
            GameSettings.EnableMouse();
        }
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
