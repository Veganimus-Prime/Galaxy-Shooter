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
    protected float _speed = 4f;
    [SerializeField]
    protected GameObject _enemyLaser;
    [SerializeField]
    protected Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    protected Animator _anim;
    protected AudioSource _audio;
    protected SpriteRenderer _sprite;
    [SerializeField]
    protected AudioClip _explosionClip;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("AudioSource is NULL!");
        }
        _sprite = GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.LogError("Sprite Renderer is NULL!");
        }
        if (_enemyID != 1)
        {
            StartCoroutine(EnemyFireRoutine());
        }
    }

    // Update is called once per frame
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
        }
    }
    public void Damage()
    {
        _lives--;
        if (_lives == 0)
        {
            _sprite.color = Color.white;
            _anim.SetTrigger("OnEnemyDeath");
            _audio.PlayOneShot(_explosionClip);
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.3f);
            Player.Instance.AddScore(10);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        StopCoroutine(EnemyFireRoutine());
        if (other.tag == "Player" || other.tag == "Laser")
        {
            Damage();
           
            if (other.tag == "Player")
            {
                Player.Instance.Damage();
            }
        }
        else if (other.tag == "Ice Beam")
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
