using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Player _player2;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject   .Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_gameManager.isCoopMod)
        {
            _player2 = GameObject.Find("Player2").GetComponent<Player>();
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position,Quaternion.identity);
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            float randomx = Random.Range(-11f, 11f);
            transform.position = new Vector3(randomx, 8f, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" )
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            if (transform.position.y < -7f)
            {
                _speed = 0;
            }
            _audioSource.Play();
            GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 2.0f);

        }
        if (other.tag == "Laser" || other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            if (transform.position.y < -7f)
            {
                _speed = 0;
            }
            _audioSource.Play();

            GetComponent<Collider>().enabled = false;
            _canFire = -10;
            Destroy(this.gameObject, 2.0f);
        }
    }
}
    