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
    private int _ammoCount = 15;
    [SerializeField]
    private int _speed;
    private int _normalSpeed = 5;
    private int _boostSpeed = 10;
    private bool _speedBoostOn = false;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _iceBeamPrefab;
    public GameObject _shield;
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    private Vector3 _tripleShotOffset = new Vector3(0, -0.5f, 0);
    private float _fireRate = 0.5f, _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _thrusterCharge = 100;
    [SerializeField]
    private bool _thrusterEnabled = true;
    public bool _isShieldActive;
    [SerializeField]
    [Header("SCORE")]
    private int _playerScore;
    [Header("VFX")]
    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;
    [SerializeField]
    private GameObject _explosion;
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioClip _powerUpClip;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audio;
    public int _fireMode = 0;
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        transform.position = new Vector3(0, -4, 0);
        _audio = GetComponent<AudioSource>();
        UIManager.Instance.UpdateAmmoCount(_ammoCount);
    }
    void Update()
    {
        Movement();
        Thrusters();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
        if (_ammoCount > 15)
        {
            _ammoCount = 15;
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
        UIManager.Instance.UpdateThrusterCharge(_thrusterCharge);
        if (_thrusterCharge > 100)
        {
            _thrusterCharge = 100;
        }
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterEnabled == true)
        {
            _speed = _boostSpeed;
            _thrusterCharge--;
        }
        else if (_speedBoostOn == false)
        {
            _speed = _normalSpeed;
        }
        if (_thrusterCharge <= 0 && _thrusterEnabled == true)
        {
            _thrusterEnabled = false;
            UIManager.Instance.thrusterCharging = true;
            StartCoroutine(ThrusterCooldown());
        }
    }
   
    void Shoot()
    {
        _canFire = Time.time + _fireRate;
        if (_ammoCount > 0)
        {
            _ammoCount--;
            UIManager.Instance.UpdateAmmoCount(_ammoCount);
            switch (_fireMode)
            {
                case 0:
                    Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(_tripleShotPrefab, transform.position + _tripleShotOffset, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(_iceBeamPrefab, transform.position + _laserOffset, Quaternion.identity);
                    break;
            }
            _audio.PlayOneShot(_laserClip, 10);
        }
        else
        {
            return;
        }
    }
    public void Damage()
    {
        if (_isShieldActive == false)
        {
            _lives--;
            CameraShake.Instance.TriggerShake();
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
            Shield.Instance.ShieldHit();
        }
    }
    void Heal()
    {
        _lives++;
        switch(_lives)
        {
            case 2:
                _leftDamage.SetActive(false);
                break;
            case 3:
                _rightDamage.SetActive(false);
                break;
        }
        UIManager.Instance.UpdateLives(_lives);
    }
    public void PowerUp(int powerUpID)
    {
        _audio.PlayOneShot(_powerUpClip);
        switch (powerUpID)
        {
            case 0://TripleShot
                _fireMode = 1;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1://SpeedBoost
                _speed = _boostSpeed;
                _speedBoostOn = true;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2://Shield
                _isShieldActive = true;
                _shield.SetActive(true);
                Shield.Instance.shieldHP = 4;
                Shield.Instance.ShieldHit();
                break;
            case 3://IceBeam
                _fireMode = 2;
                StartCoroutine(PowerUpCooldown(2));
                break;
            case 4: //+1 Life
                if(_lives <3)
                {
                    Heal();
                }
                else
                {
                    return;
                }
                break;
            case 5://Ammo
                if (_ammoCount < 15)
                {
                    _ammoCount += 5;
                }
                UIManager.Instance.UpdateAmmoCount(_ammoCount);
                break;
            case 6://Sabotage
                if (_isShieldActive == true)
                {
                    _shield.SetActive(false);
                }
                else
                {
                    Damage();
                }
                break;
            default:
                Debug.Log("Default Power Up ID");
                break;
        }
    }
    void ScreenWrap()
    {
        if (transform.position.x > 9.3f)
        {
            transform.position = new Vector3(-9.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.3f)
        {
            transform.position = new Vector3(9.3f, transform.position.y, 0);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-4f, 0));

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
            case 0://TripleShot
                yield return new WaitForSeconds(5f);
                _fireMode = 0;
                break;
            case 1://SpeedBoost
                yield return new WaitForSeconds(5f);
                _speed = _normalSpeed;
                _speedBoostOn = false;
                break;
            case 2://IceBeam
                yield return new WaitForSeconds(5f);
                _fireMode = 0;
                break;
            default:
                Debug.Log("Default Power Up ID");
                break;
        }
    }
    IEnumerator ThrusterCooldown()
    {
        while (_thrusterCharge < 100)
        {
            yield return new WaitForSeconds(5f);
            _thrusterCharge += 10;
            UIManager.Instance.UpdateThrusterCharge(_thrusterCharge);
        }
        if(_thrusterCharge ==100)
        {
            UIManager.Instance.thrusterCharging = false;
            _thrusterEnabled = true;
        }
    }
   
}
