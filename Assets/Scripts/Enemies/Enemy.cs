using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    public float speed = 4f;
    public bool isBehindPlayer = false;
    public GameObject thrusters;
    [SerializeField]
    private int _moveID, _lives = 1;
    [SerializeField]
    private bool _isFrozen = false, _isShieldActive = false;
    [SerializeField]
    private GameObject _enemyLaser, _backFireLaser, _enemyShield, _explosion;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private float _zRotation, _turnSpeed = 200f;
    [SerializeField]
    private AudioClip _explosionClip, _laserClip;
    private AudioSource _audio;
    private SpriteRenderer _sprite;
    private Quaternion _enemyRot;
    
    void Start()
    {
        _enemyRot = transform.rotation;
        _audio = GetComponentInChildren<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("AudioSource is NULL!");
        }
        _sprite = GetComponentInChildren<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.LogError("Sprite Renderer is NULL!");
        }
        StartCoroutine(EnemyFireRoutine());
        
    }
    void Update()
    {
        Movement();
        if(transform.position.y < Player.Instance.transform.position.y)
        {
            isBehindPlayer = true;
        }
    }
    void Movement()
    {
        switch (_moveID)
        {
            case 0://Down
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            case 1://Right
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                }
                break;
            case 2://Chase Player
                if (Player.Instance != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, 2f * speed * Time.deltaTime);
                    Vector3 _myPos = transform.position;
                    Vector3 _targetLocation = Player.Instance.transform.position;
                    _targetLocation.z = _myPos.z; // no 3D rotation
                    Vector3 vectorToTarget = _targetLocation - _myPos;
                    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _zRotation) * vectorToTarget;
                    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
                    Debug.DrawRay(transform.position, _targetLocation);
                }
                break;
            case 3://Left
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                if (transform.position.x < -10)
                {
                    transform.position = new Vector3(10, Random.Range(-3, 3), 0);
                }
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
                speed = 0;
                Instantiate(_explosion, transform.position - _laserOffset, _enemyRot);
                _sprite.color = Color.white;
                _audio.PlayOneShot(_explosionClip);
                Destroy(this.gameObject);
                Player.Instance.AddScore(10);
                SpawnManager.Instance.EnemyKilled();
            }
    }
    void OnTriggerEnter2D(Collider2D other)
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
            speed = 0;
            _sprite.color = Color.cyan;
            StartCoroutine(EnemyThawRoutine());
        }
        else if (other.tag == "Enemy" && _isFrozen == false)
        {
            if (_moveID == 0 && transform.position.y == 5)
            {
                transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
            }
            else if (_moveID == 1 && transform.position.x == -10)
            {
                transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
            }
        }
    }
    public void ChangeID(int newID)
    {
        _moveID = newID;
    }
    public void EnemyFire()
    {
                if (isBehindPlayer == true && _backFireLaser != null)
                {
                    Instantiate(_backFireLaser, transform.position - _laserOffset, _enemyRot);
                }
                else
                {
                    Instantiate(_enemyLaser, transform.position - _laserOffset, _enemyRot);
                }
        
        _audio.PlayOneShot(_laserClip);
    }
    IEnumerator EnemyFireRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        EnemyFire();
    }
    IEnumerator EnemyThawRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isFrozen = false;
        speed = 4f;
        _sprite.color = Color.white;
    }
}
