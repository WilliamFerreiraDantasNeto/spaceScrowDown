using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); //Menu Scene
    }
    public void Quilt()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(false);
        }
        Time.timeScale = 1;
    }
}

