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
    [SerializeField] EventList[] eventList;
    [SerializeField] Flags flag;
    public float currentTime = 0;
    int eventInd = 0;
    ItemList orig;

    [System.Serializable]
    public struct EventList
    {
        public DialogueNodes eve;
        public float time;
    }
    void OnEnable()
    {
        Time.timeScale = 1;
        orig = new ItemList();
        orig.itemStorage = new GameObject[inventory.itemStorage.Length];
        for (int i = 0; i < inventory.itemStorage.Length; i++)
        {
            GameObject temp = inventory.itemStorage[i];
            orig.itemStorage[i] = temp;
        }
        orig.money = inventory.money;
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
            if (eventInd < eventList.Length && currentTime >= eventList[eventInd].time)
            {
                if (eventList[eventInd].eve.a == EnumLib.available.Always || flag.CheckTag(eventList[eventInd].eve.flag))
                {
                    GameObject.FindGameObjectWithTag("DialogueHandler").GetComponent<DialogueHandler>().TraverseDialogueTree(eventList[eventInd].eve);
                }
                eventInd++;
            }
        }
        EndGame();
    }

    void EndGame()
    {
        inventory.Reset(orig);
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
    public void ResetInventory()
    {
        inventory.Reset(orig);
    }
}
