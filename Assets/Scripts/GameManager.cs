using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    private PlayerControl playerControl;
    [SerializeField]
    public bool isCoopMod = false;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private UIManager _uiManager;
    private int _score = 0;

    private void Awake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }
    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void Update()
    {
        //if the r key was pressed
        // restart the current scene
        if (playerControl.PlayerMain.Restart.triggered && _isGameOver == true)
        {
            if (isCoopMod)
            {
                SceneManager.LoadScene(2); //Current Game Scene
            }
            else
            {
                SceneManager.LoadScene(1); //Current Game Scene
            }
            
        }
        if (playerControl.PlayerMain.Pause.triggered)
        {
            if (_pauseMenu != null)
            {
                _pauseMenu.SetActive(true);
            }
            PauseGame();
        }
        
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
