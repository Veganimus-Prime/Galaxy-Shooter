using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    [SerializeField]
    private int _enemyID;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private bool _isFrozen = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private float _speed = 4f, _rotationSpeed = 2f;
    [SerializeField]
    protected GameObject _enemyLaser, _enemyShield;
    [SerializeField]
    protected Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    protected Animator _anim;
    protected AudioSource _audio;
    protected SpriteRenderer _sprite;
    [SerializeField]
    protected AudioClip _explosionClip, _laserClip;
    void Start()
    {
        if (_enemyID == 3)
        {
            _enemyShield.SetActive(true);
            _isShieldActive = true;
        }
        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
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
    }
    void Movement()
    {
        switch (_enemyID)
        {
            case 0:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            case 1:
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                }
                break;
            case 2:
                if (Player.Instance != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, 2f * _speed * Time.deltaTime);
                }
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                   
                }
                break;
            case 3:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
        }
    }
    public void Damage()
    {
            _lives--;
            if (_lives == 0)
            {
                _speed = 0;
                Destroy(GetComponent<Collider2D>());               
                StopCoroutine(EnemyFireRoutine());
                _sprite.color = Color.white;
                _anim.SetTrigger("OnEnemyDeath");
                _audio.PlayOneShot(_explosionClip);
                Destroy(this.gameObject, 1.3f);
                Player.Instance.AddScore(10);
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
            _speed = 0;
            _sprite.color = Color.cyan;
            StartCoroutine(EnemyThawRoutine());
        }
    }
    public void ChangeID(int newID)
    {
        _enemyID = newID;
    }
    public void EnemyFire()
    {
        Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
        _audio.PlayOneShot(_laserClip);
    }
    protected IEnumerator EnemyFireRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3));
        switch (_enemyID)
        {
            case 0:
                Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
                break;
            case 1:
                Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.Euler(0,0, 90f));
                break;
            default:
                Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
                break;
        }
        _audio.PlayOneShot(_laserClip);
    }
    IEnumerator EnemyThawRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isFrozen = false;
        _speed = 4f;
        _sprite.color = Color.white;
    }
    /*IEnumerator Kamikaze()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeID(2);
        yield return new WaitForSeconds(1f);
        StopCoroutine(EnemyFireRoutine());
        _sprite.color = Color.white;
        _anim.SetTrigger("OnEnemyDeath");
        _audio.PlayOneShot(_explosionClip);
        _speed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 0.5f);
        Player.Instance.AddScore(10);
    }*/
}
