using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class GameSettings
    {
        public static float generalVolume = 1;
        public static bool isMouseActive = true;
        public static Scene actualScene;
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
        public static void ActiveScene()
        {
            actualScene = SceneManager.GetActiveScene();
        }
        public static bool gamePaused;
        public static void PauseGame()
        {
            if (gamePaused)
            {
                Time.timeScale = 0;
                isMouseActive = true;
            }
            else
            {
                Time.timeScale = 1;
                isMouseActive = false;
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
        public static void EnableMouse()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }       
        public static void DisableMouse()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}
