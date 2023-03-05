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
            SceneManager.LoadScene(1); //Current Game Scene
        }
        if (playerControl.PlayerMain.Quilt.triggered)
        {
            Quilt();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
    private void Quilt()
    {
        Application.Quit();
    }
}
