using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AudioSettings;

public class UIManager : MonoBehaviour
{
    // handle to Texte
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg2;
    [SerializeField]
    private Sprite[] _liveSprites2;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;
    private GameManager _gameManager;
    [SerializeField]
    private bool _player1Death = false, _player2Death = false;
    [SerializeField]
    private GameObject _mobile;
    [SerializeField]
    private Text _HiscoreText;
    private int _score, _hiScore = 0;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _mobile.SetActive(true);
#else
        _mobile.SetActive(false);
#endif
        _hiScore = PlayerPrefs.GetInt("HiScore", 0);
        _HiscoreText.text = "Hi-Score: " + _hiScore.ToString();
        _scoreText.text = "Score: " + 0;
        _GameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _score = playerScore;
        _scoreText.text = "Score: " + _score.ToString();
    }

    public void CheckForHiScore()
    {
        if (_score > _hiScore)
        {
            _hiScore = _score;
            PlayerPrefs.SetInt("HiScore", _hiScore);
            _HiscoreText.text = "Hi-Score: " + _score.ToString();
        }
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        _LivesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            _player1Death = true;
            GameOverSequence();
        }
        
    }
    public void UpdateLivesPlayer2(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        _LivesImg2.sprite = _liveSprites2[currentLives];
        if (currentLives == 0)
        {
            _player2Death = true;
            GameOverSequence();
        }
    }
    public void GameOverSequence()
    {
        if (_gameManager.isCoopMod)
        {
            if(_player1Death && _player2Death)
            {
                _gameManager.GameOver();
                _GameOverText.gameObject.SetActive(true);
                _RestartText.gameObject.SetActive(true);
                StartCoroutine(GameOverFlikerRoutine());
            }
        }
        else
        {
            _gameManager.GameOver();
            _GameOverText.gameObject.SetActive(true);
            _RestartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlikerRoutine());
        }
        

    }

    
    IEnumerator GameOverFlikerRoutine()
    {
        while (true)
        {
            _GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
