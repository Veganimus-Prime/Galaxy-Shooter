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
                Debug.Log("Player is NULL!");
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
    private int _auxillaryCharge = 100;
    [SerializeField]
    private bool _auxillaryEnabled = true;
    public bool isShieldActive;
    public bool isAuxShieldActive = false;
    public bool powerUpShield = false;
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
    public bool _magnetOn = false;
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
        Magnet();
        AuxShield();

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
    void Auxillary()
    {
        UIManager.Instance.UpdateAuxillaryCharge(_auxillaryCharge);
        if (_auxillaryCharge > 100)
        {
            _auxillaryCharge = 100;
        }
        if (_auxillaryCharge <= 0 && _auxillaryEnabled == true)
        {
            _auxillaryEnabled = false;
            UIManager.Instance.auxillaryCharging = true;
            StartCoroutine(AuxillaryCooldown());
        }
    }
    void Magnet()
    {
        Auxillary();
        if (Input.GetKey(KeyCode.C) && _auxillaryEnabled == true)
        {
            _magnetOn = true;
            _auxillaryCharge--;
        }
        else
        {
            _magnetOn = false;
        }
    }
    void Thrusters()
    {
        Auxillary();
       
        if (Input.GetKey(KeyCode.LeftShift) && _auxillaryEnabled == true)
        {
            _speed = _boostSpeed;
            _auxillaryCharge--;
        }
        else if (_speedBoostOn == false)
        {
            _speed = _normalSpeed;
        }
       
    }
    void AuxShield()
    {
        Auxillary();
        if (Input.GetKey(KeyCode.RightShift) && _auxillaryEnabled == true)
        {
            isAuxShieldActive = true;
            _shield.SetActive(true);
            _auxillaryCharge--;
        }
        else if(powerUpShield == false)
        {
            isAuxShieldActive = false;
            _shield.SetActive(false);
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
        if (isShieldActive == false && isAuxShieldActive == false )
        {
            _lives--;
            CameraShake.Instance.TriggerShake();
            switch (_lives)
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
        else if (Shield.Instance.shieldHP > 0)
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
        switch (powerUpID)
        {
            case 0://TripleShot
                _audio.PlayOneShot(_powerUpClip);
                _fireMode = 1;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1://SpeedBoost
                _audio.PlayOneShot(_powerUpClip);
                _speed = _boostSpeed;
                _speedBoostOn = true;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2://Shield
                _audio.PlayOneShot(_powerUpClip);
                powerUpShield = true;
                isShieldActive = true;
                _shield.SetActive(true);
                Shield.Instance.shieldHP = 4;
                Shield.Instance.ShieldHit();
                break;
            case 3://IceBeam
                _audio.PlayOneShot(_powerUpClip);
                _fireMode = 2;
                StartCoroutine(PowerUpCooldown(2));
                break;
            case 4: //+1 Life
                _audio.PlayOneShot(_powerUpClip);
                if (_lives <3)
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
                _audio.PlayOneShot(_explosionClip);
                if (isShieldActive == true)
                {
                    isShieldActive = false;
                    _shield.SetActive(false);
                    Shield.Instance.shieldHP = 0;
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
    IEnumerator AuxillaryCooldown()
    {
        while (_auxillaryCharge < 100)
        {
            yield return new WaitForSeconds(5f);
            _auxillaryCharge += 10;
            UIManager.Instance.UpdateAuxillaryCharge(_auxillaryCharge);
        }
        if(_auxillaryCharge ==100)
        {
            UIManager.Instance.auxillaryCharging = false;
            _auxillaryEnabled = true;
        }
    }
}
