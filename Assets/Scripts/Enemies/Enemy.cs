using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    public float speed = 4f;
    public bool isBehindPlayer = false;
    public GameObject thrusters;
    [SerializeField]
    private int _enemyID;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private bool _isFrozen = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _enemyLaser,_backFireLaser, _enemyShield;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    [SerializeField]
    private AudioClip _explosionClip, _laserClip;
    [SerializeField]
    private GameObject _closest;
    private Animator _anim;
    private AudioSource _audio;
    private SpriteRenderer _sprite;
    
    void Start()
    {
        FindClosestTarget();
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
        if(transform.position.y < _closest.transform.position.y)
        {
            isBehindPlayer = true;
        }
    }
    void Movement()
    {
        switch (_enemyID)
        {
            case 0:
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                if (transform.position.y < -6)
                {
                    transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
                }
                break;
            case 1:
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                }
                break;
            case 2:
                if (Player.Instance != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, 2f * speed * Time.deltaTime);
                }
                if (transform.position.x > 10)
                {
                    transform.position = new Vector3(-10, Random.Range(-3, 3), 0);
                   
                }
                break;
            case 3:
                transform.Translate(Vector3.down * speed * Time.deltaTime);
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
                if (thrusters != null)
                {
                thrusters.SetActive(false);
                }
                speed = 0;
                Destroy(GetComponent<Collider2D>());               
                StopCoroutine(EnemyFireRoutine());
                _sprite.color = Color.white;
                _anim.SetTrigger("OnEnemyDeath");
                _audio.PlayOneShot(_explosionClip);
                Destroy(this.gameObject, 1.3f);
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
    }
    public void ChangeID(int newID)
    {
        _enemyID = newID;
    }
    public void EnemyFire()
    {
        switch (_enemyID)
        {
            case 0:
                if (isBehindPlayer == true && _backFireLaser != null)
                {
                    Instantiate(_backFireLaser, transform.position - _laserOffset, Quaternion.identity);
                }
                else
                {
                    Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
                }

                break;
            case 1:
                Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.Euler(0, 0, 90f));
                break;
            default:
                Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
                break;
        }
        _audio.PlayOneShot(_laserClip);
    }
    IEnumerator EnemyFireRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        EnemyFire();
    }
    private GameObject FindClosestTarget()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("Player");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject target in targets)
        {
            Vector3 difference = target.transform.position - position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance)
            {
                _closest = target;
                distance = currentDistance;
            }
        }
        if (_closest != null)
        {
            Debug.Log(_closest.name);

        }
        return _closest;
    }
    IEnumerator EnemyThawRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isFrozen = false;
        speed = 4f;
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
