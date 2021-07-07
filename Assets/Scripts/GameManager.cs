using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameManager
{
    public class GameSettings
    {
        public static float generalVolume = 1;
        public static void ChangeVolume(float value)
        {
            foreach (AudioSource audioSource in GameObject.FindObjectsOfType<AudioSource>())
            {
                audioSource.volume = value;
            }
        }
        public static void ChangeResolution(int resolutionIndex)
        {
            Resolution[] resolutions = Screen.resolutions;
            Resolution newResolution = resolutions[resolutionIndex];
            Screen.SetResolution(newResolution.width, newResolution.height, true);
        }
        public static void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        public static void ChangeScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        public static bool gamePaused;
        public static void PauseGame()
        {
            if (gamePaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                gamePaused = false;
            }
        }
        public static void CloseGame()
        {
            Application.Quit();
        }

        public static void PushAnyKey(GameObject panelFrom, GameObject panelTo)
        {
            panelFrom.SetActive(false);
            panelTo.SetActive(true);
        }
    }
}
