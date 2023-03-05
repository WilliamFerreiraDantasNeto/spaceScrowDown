using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.AudioSettings;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mobile;
    private void Start()
    {
#if UNITY_ANDROID
        _mobile.SetActive(false);
#else
        _mobile.SetActive(true);
#endif
    }
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1); //Game Scene
    }
    public void LoadCoOpGame()
    {
        SceneManager.LoadScene(2); //Game Scene
    }
}
