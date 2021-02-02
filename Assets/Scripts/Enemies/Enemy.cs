using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Enemy: MonoBehaviour
{
    public enum EnemyMovement
    {
        Down, Right, Chase, Left
    }
    [SerializeField]
    private EnemyMovement _moveID;
    public enum EnemyType
    {
        Basic, Left, Right, Ram, Shield, Evader, Backfire
    }
    [SerializeField]
    private EnemyType _enemyType;
    [SerializeField]
    private Sprite[] _enemySprite = new Sprite[5];
    public float Speed { get; private set; } = 4f;
    private float CanFire { get; set; } = -1.0f;
    private float FireRate { get; set; } = 1.0f;
    public GameObject thrusters;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private int _scoreValue = 10;
    [SerializeField]
    private bool _isFrozen = false, _isShieldActive = false;
    [SerializeField]
    private GameObject _enemyLaser, _backFireLaser, _enemyShield, _explosion;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private float _zRotation;
    [SerializeField]
    private static float _turnSpeed = 200f;
    [SerializeField]
    private AudioClip _explosionClip, _laserClip;
    private AudioSource _audio;
    //private SpriteRenderer _sprite;
    private Quaternion _enemyRot;
    [SerializeField]
    private LayerMask targetPlayer, targetPowerUp;
    private RaycastHit2D _hit;
    private WaitForSeconds _thawCoolDown;
    
    void Start()
    {
        _thawCoolDown = new WaitForSeconds(3f);
        _enemyRot = transform.rotation;
        _audio = GetComponentInChildren<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("AudioSource is NULL!");
        }
       // _sprite = GetComponentInChildren<SpriteRenderer>();
        /*if (_sprite == null)
        {
            Debug.LogError("Sprite Renderer is NULL!");
        }*/
    }
    void Update()
    {
        AI();
    }
    void AI()
    {
        switch(_enemyType)
        {
            case EnemyType.Basic:
                //Debug.DrawRay(transform.position, Vector3.down * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.up, 7f, targetPlayer);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector2.right * Speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            case EnemyType.Right:
                //Debug.DrawRay(transform.position, Vector3.right * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.right, 7f, targetPlayer);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                }
                break;
            case EnemyType.Left:
                //Debug.DrawRay(transform.position, Vector3.left * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.left, 7f, targetPlayer);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.x < -10)
                {
                    transform.position = new Vector3(10, Random.Range(-3, 3), 0);
                }
                break;
            case EnemyType.Ram:
                //Debug.DrawRay(transform.position, Vector3.down * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.down, 7f, targetPlayer);
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, 2f * Speed * Time.deltaTime);
                    Vector3 _myPos = transform.position;
                    Vector3 _targetLocation = Player.Instance.transform.position;
                    _targetLocation.z = _myPos.z; // no 3D rotation
                    Vector3 vectorToTarget = _targetLocation - _myPos;
                    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _zRotation) * vectorToTarget;
                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
                    //Debug.DrawRay(transform.position, _targetLocation);
                }
                break;
            case EnemyType.Shield:
                //Debug.DrawRay(transform.position, Vector3.down * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.down, 7f, targetPowerUp);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            case EnemyType.Evader:
                //Debug.DrawRay(transform.position, Vector3.right * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.right, 7f, targetPlayer);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                }
                break;
            case EnemyType.Backfire:
                //Debug.DrawRay(transform.position, Vector3.up * 7f, Color.red);
                _hit = Physics2D.Raycast(transform.position, Vector2.up, 7f, targetPlayer);
                if (_hit.collider != null)
                {
                    //Debug.Log("Ray has hit:" + _hit.collider.gameObject.name);
                    StartCoroutine(EnemyFireRoutine());
                }
                transform.Translate(Vector3.up * Speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            default:
                break;
        }
    }
    
    public void Damage()
    {
            _lives--;
            if (_lives == 0)
            {
                if (thrusters != null)
                {
                thrusters.SetActive(false);
                }
                Speed = 0;
                Destroy(this.gameObject);
                Instantiate(_explosion, transform.position, Quaternion.identity);
                //_sprite.color = Color.white;
                _audio.PlayOneShot(_explosionClip);
                UIManager.Instance.UpdateScore(_scoreValue);
                SpawnManager.Instance.EnemyKilled();
            }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" || other.tag == "Laser")
        {
            if (_isShieldActive == false)
            {
                Damage();
            }
            else
            {
                _isShieldActive = false;
                _enemyShield.SetActive(false);
            }

            if (other.tag == "Player")
            {
                Player.Instance.Damage();
            }
        }
        else if (other.tag == "Ice Beam" && _isShieldActive == false)
        {
            StopCoroutine(EnemyFireRoutine());
            _isFrozen = true;
            Speed = 0;
           // _sprite.color = Color.cyan;
            StartCoroutine(EnemyThawRoutine());
        }
        else if (other.tag == "Enemy" && _isFrozen == false)
        {
            if (_moveID == 0 && transform.position.y == 5)
            {
                transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
            }
            else if ((int)_moveID == 1 && transform.position.x == -10)
            {
                transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
            }
        }
    }
    public void EnemyFire()
    {
        CanFire = Time.time + FireRate;
            switch (_enemyType)
            {
                case EnemyType.Backfire:
                    Instantiate(_backFireLaser, transform.position - _laserOffset, _enemyRot);
                    break;
                default:
                    Instantiate(_enemyLaser, transform.position - _laserOffset, _enemyRot);
                    break;
            }
        _audio.PlayOneShot(_laserClip);
    }
    public int AssignID(int id)
    {
        Debug.Log(id);
        return id;
        
    }
    IEnumerator EnemyFireRoutine()
    {
        while (Time.time > CanFire)
        {
            EnemyFire();
        }
        yield break;
    }
    IEnumerator EnemyThawRoutine()
    {
        yield return _thawCoolDown;
        _isFrozen = false;
        Speed = 4f;
        //_sprite.color = Color.white;
    }
}
