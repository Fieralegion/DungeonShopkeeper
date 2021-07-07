using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField] float totalMinutes, moneyTotal;
    [SerializeField] GameObject winScreen, loseScreen;
    [SerializeField] Button winRestart, loseRestart, winMainMenu, loseMainMenu;
    [SerializeField] ItemList inventory;
    public float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        winRestart.onClick.AddListener(() => GameSettings.ChangeScene(1));
        loseRestart.onClick.AddListener(() => GameSettings.ChangeScene(1));
        winMainMenu.onClick.AddListener(() => GameSettings.ChangeScene(0));
        loseMainMenu.onClick.AddListener(() => GameSettings.ChangeScene(0));
        StartCoroutine(TimePassing());
    }

    IEnumerator TimePassing()
    {
        for (int i = 0; i < totalMinutes * 60; i++)
        {
            yield return new WaitForSeconds(1);
            currentTime++;
        }
        EndGame();
    }

    void EndGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (inventory.money >= moneyTotal)
        {
            winScreen.SetActive(true);
        }
        else
        {
            loseScreen.SetActive(true);
        }
    }
}
