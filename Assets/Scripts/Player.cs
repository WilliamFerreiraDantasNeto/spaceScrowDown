using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldtActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _LeftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    private Animator _anim;
    private CharacterController controller;
    private PlayerControl playerControl;
    private GameManager _gameManager;
    [SerializeField]
    private bool _isPlayerTwo = false;
    private void Awake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        controller = GetComponent<CharacterController>();
        playerControl.Enable();
    }
    private void OnDisable()
    {
        playerControl.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
       
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager.isCoopMod == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manger is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CauculateMovement();
        if (_isPlayerTwo)
        {
            if (playerControl.PlayerTwo.Fire.triggered && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        else
        {
            if (playerControl.PlayerMain.Fire.triggered && Time.time > _canFire)
            {
                FireLaser();
            }

        }
    }
    void CauculateMovement()
    {
        Vector2 movementInput = playerControl.PlayerMain.Move.ReadValue<Vector2>();
        if (_isPlayerTwo)
        {
            movementInput = playerControl.PlayerTwo.Move.ReadValue<Vector2>();
        }
            
        Vector3 move = new Vector3(movementInput.x, movementInput.y, 0f);
        
        if (_isSpeedBoostActive)
        {
            controller.Move(move * Time.deltaTime * _speed * _speedMultiplier);
        }
        else
        {
            controller.Move(move * Time.deltaTime * _speed);
        }
        if (movementInput.x > 0)
        {
            _anim.SetBool("Turn_left", true);

        }
        else if (movementInput.x == 0)
        {
            _anim.SetBool("Turn_left", false);
            _anim.SetBool("Turn_right", false);
        }
        else if (movementInput.x < 0)
        {
            _anim.SetBool("Turn_right", true);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position + new Vector3(-0.19f, 0.2f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldtActive == true)
        {
            _isShieldtActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_isPlayerTwo)
        {
            _uiManager.UpdateLivesPlayer2(_lives);
        }
        else
        {
            _uiManager.UpdateLives(_lives);
        }
        
        if (_lives == 2)
        {
            _LeftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        else if (_lives < 1)
        {
            if (_isPlayerTwo)
            {
                _spawnManager.OnPlayerDeath(2);
            }
            else
            {
                _spawnManager.OnPlayerDeath(1);
            }
            
            Destroy(this.gameObject);
            _uiManager.CheckForHiScore();
        }
        GetComponent<Collider>().enabled = false;
        ColiderRoutine();
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    IEnumerator ColiderRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Collider>().enabled = true;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false; 
    }

    public void ShieldActive()
    {
        _isShieldtActive = true;
        _shieldVisualizer.SetActive(true);
    }

    
}