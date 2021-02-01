using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SpriteRenderer))]
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
    private enum FireMode
    {
        Normal, TripleShot, IceBeam, LocNar
    }
    [SerializeField]
    private FireMode _fireMode;
    [SerializeField][Range(0, 5)]
    private int _ammoCount = 5;
    [SerializeField][Range(0, 15)]
    private int _ammoReserve = 15;
    [SerializeField][Range(1, 10)]
    private int _speed;
    private static int NormalSpeed { get; } = 5;
    private static int BoostSpeed { get; } = 10;
    private bool _speedBoostOn = false;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _iceBeamPrefab, _homingPrefab;
    public GameObject _shield;
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    private Vector3 _tripleShotOffset = new Vector3(0, -0.5f, 0);
    private float FireRate { get; set; } = 0.5f;
    private float CanFire { get; set; } = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField][Range(0, 100)]
    private float _auxillaryCharge = 100f;
    
    public bool AuxillaryEnabled { get; private set; } = true;
    public bool IsShieldActive { get; set; } = false;
    public bool AuxShieldOn { get; private set; } = false;
    public bool PowerUpShield { get; set; } = false;
    public bool MagetOn { get; private set; } = false;
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
    [SerializeField]
    private AudioClip _laserClip, _tripleShotClip, _iceBeamClip, _homingClip, _magnetClip;
    [SerializeField]
    private AudioClip _powerUpClip;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audio;
    private WaitForSeconds _defaultCoolDown;
    private WaitForSeconds _reloadCoolDown;
    private void Awake()
    {
        _instance = this;
        _defaultCoolDown = new WaitForSeconds(5f);
        _reloadCoolDown = new WaitForSeconds(2f);
    }

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _audio = GetComponent<AudioSource>();
        UIManager.Instance.UpdateAmmoCount(_ammoCount);
    }
    void Update()
    {
        Movement();
        Thrusters();
        Magnet();
        AuxShield();
        ChangeFireMode();
        Reload();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > CanFire)
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
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0));
    }
    void ChangeFireMode()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _fireMode++;
        }
        if((int)_fireMode >3)
        {
            _fireMode = 0;
        }
    }
    public void AuxillaryRecharge()
    {
        _auxillaryCharge += 10;
    }
    void Auxillary()
    {
        GaugeMeter.Instance.UpdateGauge(_auxillaryCharge);
        UIManager.Instance.UpdateAuxillaryCharge(_auxillaryCharge);
        if (_auxillaryCharge > 100)
        {
            _auxillaryCharge = 100;
        }
        if (_auxillaryCharge <= 0 && AuxillaryEnabled == true)
        {
            AuxillaryEnabled = false;
            UIManager.Instance.auxillaryCharging = true;
            StartCoroutine(AuxillaryCooldown());
        }
    }
    void Magnet()
    {
        Auxillary();
        if (Input.GetKey(KeyCode.C) && AuxillaryEnabled == true)
        {
            MagetOn = true;
            _auxillaryCharge-=0.5f;
            _audio.PlayOneShot(_magnetClip);
        }
        else
        {
            MagetOn = false;
        }
    }
    void Thrusters()
    {
        Auxillary();
       
        if (Input.GetKey(KeyCode.LeftShift) && AuxillaryEnabled == true)
        {
            _speed = BoostSpeed;
            _auxillaryCharge -= 0.5f;
        }
        else if (_speedBoostOn == false)
        {
            _speed = NormalSpeed;
        }
       
    }
    void AuxShield()
    {
        Auxillary();
        if (Input.GetKey(KeyCode.RightShift) && AuxillaryEnabled == true)
        {
            AuxShieldOn = true;
            _shield.SetActive(true);
            _auxillaryCharge -= 0.5f;
        }
        else if(PowerUpShield == false)
        {
            AuxShieldOn = false;
            _shield.SetActive(false);
        }
    }
    void Reload()
    {
        if (_ammoCount > 5)
        {
            _ammoCount = 5;
        }
        if (_ammoReserve > 15)
        {
            _ammoReserve = 15;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_ammoCount == 0 && _ammoReserve >= 5)
            {
                StartCoroutine(ReloadTimer());
            }
            else
                return;
        }
    }
   
    void Shoot()
    {
        CanFire = Time.time + FireRate;
        if (_ammoCount > 0)
        {
            _ammoCount--;
            UIManager.Instance.UpdateAmmoCount(_ammoCount);
            switch ((int)_fireMode)
            {
                case 0:
                    Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                    _audio.PlayOneShot(_laserClip, 10);
                    break;
                case 1:
                    Instantiate(_tripleShotPrefab, transform.position + _tripleShotOffset, Quaternion.identity);
                    _audio.PlayOneShot(_tripleShotClip, 10);
                    break;
                case 2:
                    Instantiate(_iceBeamPrefab, transform.position + _laserOffset, Quaternion.identity);
                    _audio.PlayOneShot(_iceBeamClip, 10);
                    break;
                case 3:
                    Instantiate(_homingPrefab, transform.position + _laserOffset, Quaternion.identity);
                    _audio.PlayOneShot(_homingClip, 10);
                    break;
            }
        }
        else
        {
            return;
        }
    }
    public void Damage()
    {
        if (IsShieldActive == false && AuxShieldOn == false )
        {
            _lives--;
            CameraShake.Instance.TriggerShake();
            switch (_lives)
            {
                case 0:
                    _audio.PlayOneShot(_explosionClip);
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    Destroy(gameObject);
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
                _fireMode = FireMode.TripleShot;
                StartCoroutine(PowerUpCooldown(0));
                break;
            case 1://SpeedBoost
                _audio.PlayOneShot(_powerUpClip);
                _speed = BoostSpeed;
                _speedBoostOn = true;
                StartCoroutine(PowerUpCooldown(1));
                break;
            case 2://Shield
                _audio.PlayOneShot(_powerUpClip);
                PowerUpShield = true;
                IsShieldActive = true;
                _shield.SetActive(true);
                Shield.Instance.shieldHP = 4;
                Shield.Instance.ShieldHit();
                break;
            case 3://IceBeam
                _audio.PlayOneShot(_powerUpClip);
                _fireMode = FireMode.IceBeam;
                StartCoroutine(PowerUpCooldown(0));
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
                if (_ammoReserve < 15)
                {
                    _audio.PlayOneShot(_powerUpClip);
                    _ammoReserve += 5;
                }
                else if(_ammoReserve >15)
                {
                    return;
                }
                UIManager.Instance.UpdateReserveAmmo(_ammoReserve);
                break;
            case 6://Sabotage
                _audio.PlayOneShot(_explosionClip);
                if (IsShieldActive == true)
                {
                    IsShieldActive = false;
                    _shield.SetActive(false);
                    Shield.Instance.shieldHP = 0;
                }
                else
                {
                    Damage();
                }
                break;
            case 7://LOC-NAR
                _audio.PlayOneShot(_powerUpClip);
                _fireMode = FireMode.LocNar;
                StartCoroutine(PowerUpCooldown(0));
                break;
            default:
                //Debug.Log("Default Power Up ID");
                break;
        }
    }
    public void ScreenWrap(int position)
    {
        switch(position)
        {
            case 0:
                transform.position = new Vector3(-9.3f, transform.position.y, 0);
                break;
            case 1:
                transform.position = new Vector3(9.3f, transform.position.y, 0);
                break;
        }
    }
    /*public void AddScore(int points)
    {
        _playerScore += points;
        UIManager.currentScore = _playerScore;
    }*/
    IEnumerator PowerUpCooldown(int powerUpID)
    {
        switch (powerUpID)
        {
            case 0://Fire Mode
                yield return _defaultCoolDown;
                _fireMode = 0;
                break;
            case 1://SpeedBoost
                yield return _defaultCoolDown;
                _speed = NormalSpeed;
                _speedBoostOn = false;
                break;
            default:
                //Debug.Log("Default Power Up ID");
                break;
        }
    }
    IEnumerator AuxillaryCooldown()
    {
        while (_auxillaryCharge < 100)
        {
            yield return _defaultCoolDown;
            _auxillaryCharge += 10;
            UIManager.Instance.UpdateAuxillaryCharge(_auxillaryCharge);
        }
        if(_auxillaryCharge ==100)
        {
            UIManager.Instance.auxillaryCharging = false;
            AuxillaryEnabled = true;
        }
    }
    IEnumerator ReloadTimer()
    {
        yield return _reloadCoolDown;
        _ammoCount += 5;
        _ammoReserve -= 5;
        UIManager.Instance.UpdateAmmoCount(_ammoCount);
        UIManager.Instance.UpdateReserveAmmo(_ammoReserve);
    }
}
