using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Player is NULL!");
            }
            return _instance;
        }
    }
    [SerializeField]
    private float _speed = 5;
    private float _vBoundTop = 0f;
    private float _vBoundBottom = -4f;
    private float _hBoundRight = 9.3f;
    private float _hBoundLeft = -9.3f;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _shield;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0,0.5f,0);
    [SerializeField]
    private Vector3 _tripleShotOffset = new Vector3(0, -0.5f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3, _fireMode = 0;
    [SerializeField]
    private float _thrusterCharge = 100;
    [SerializeField]
    private bool _thrusterEnabled = true;
    [SerializeField]
    private bool _isShieldActive;
    [SerializeField]
    [Header("SCORE")]
    private int _playerScore;
    [SerializeField]
    private GameObject _rightDamage, _leftDamage, _explosion;
    [SerializeField]
    private AudioClip _laserClip, _powerUpClip, _explosionClip;
    private AudioSource _audio;
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        transform.position = new Vector3(0, -4, 0);
        _audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        Movement();
        Thrusters();
       
        if(Input.GetKeyDown(KeyCode.Space)&& Time.time > _canFire)
        {
            Shoot();
        }

    }
    void Movement()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        ScreenWrap();
    }
    void Thrusters()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterEnabled == true)
        {
            _speed = 8;
            _thrusterCharge--;
        }
        else
        {
            _speed = 5;
        }
        if(_thrusterCharge <= 0)
        {
            _thrusterEnabled = false;
        }
       /* else if (_thrusterCharge <= 0)
        {
            if(_thrusterCharge <0)
            {
                _thrusterCharge = 0;
            }
            _thrusterEnabled = false;
            _speed = 5;
            StartCoroutine(ThrusterCoolDown());
        }*/
    }
    /*IEnumerator ThrusterCoolDown()
    {
        yield return new WaitForSeconds(10f);
        _thrusterEnabled = true;
        _thrusterCharge = 100;
    }*/
    void Shoot()
    {
        _canFire = Time.time + _fireRate;
        switch(_fireMode)
        {
            case 0:
                Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                break;
            case 1:
                Instantiate(_tripleShotPrefab, transform.position + _tripleShotOffset, Quaternion.identity);
                break;
        }
        _audio.PlayOneShot(_laserClip, 10);
    }
    public void Damage()
    {
        if (_isShieldActive == false)
        {
            _lives--;
            switch(_lives)
            {
                case 0:
                    _audio.PlayOneShot(_explosionClip);
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                    SpawnManager.Instance.OnPlayerDeath();
                    break;
                case 1:
                    _leftDamage.SetActive(true);
                    break;
                case 2:
                    _rightDamage.SetActive(true);
                    break;

            }
            UIManager.Instance.UpdateLives(_lives);
        }
        else if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
        }
    }
    public void PowerUp(int powerUpID)
    {
        _audio.PlayOneShot(_powerUpClip);
        switch (powerUpID)
        {
            case 0:
                _fireMode = 1;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1:
                _speed += 2;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2:
                _isShieldActive = true;
                _shield.SetActive(true);
                break;
            default:
                Debug.Log("Default Power Up ID");
                break;
        }
    }
    void ScreenWrap()
    {
        if (transform.position.x > _hBoundRight)
        {
            transform.position = new Vector3(_hBoundLeft, transform.position.y, 0);
        }
        else if (transform.position.x < _hBoundLeft)
        {
            transform.position = new Vector3(_hBoundRight, transform.position.y, 0);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _vBoundBottom, _vBoundTop));

    }
    public void AddScore(int points)
    {
        _playerScore += points;
        UIManager.Instance.UpdateScore(_playerScore);
    }
    IEnumerator PowerUpCooldown(int powerUpID)
    {
        switch (powerUpID)
        {
            case 0:
                yield return new WaitForSeconds(5f);
                _fireMode = 0;
                break;
            case 1:
                yield return new WaitForSeconds(5f);
                _speed -= 2;
                break;
            default:
                Debug.Log("Default Power Up ID");
                break;
        }
    }
   
}
