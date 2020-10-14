using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    [SerializeField]
    protected int _enemyID;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    protected bool _isFrozen = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    protected float _speed = 4f;
    [SerializeField]
    protected GameObject _enemyLaser, _enemyShield;
    [SerializeField]
    protected Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    protected Animator _anim;
    protected AudioSource _audio;
    protected SpriteRenderer _sprite;
    [SerializeField]
    protected AudioClip _explosionClip;
   
    void Start()
    {
        if(_enemyID == 3)
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
        if (_enemyID != 1)
        {
            StartCoroutine(EnemyFireRoutine());
        }
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
                _speed = 2.5f;
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
        if (_isShieldActive == false)
        {
            _lives--;
            if (_lives == 0)
            {
                StopCoroutine(EnemyFireRoutine());
                _sprite.color = Color.white;
                _anim.SetTrigger("OnEnemyDeath");
                _audio.PlayOneShot(_explosionClip);
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 1.3f);
                Player.Instance.AddScore(10);
            }
        }
        else
        {
            _isShieldActive = false;
            _enemyShield.SetActive(false);
            ChangeID(2);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player" || other.tag == "Laser")
        {
            Damage();
           
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
    }
    protected IEnumerator EnemyFireRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3));
        Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
    }
    IEnumerator EnemyThawRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isFrozen = false;
        _speed = 4f;
        _sprite.color = Color.white;
    }
}
