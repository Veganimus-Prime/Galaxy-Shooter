using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private int _laserID, _moveID;
    [SerializeField]
    private GameObject _explosion;
    void Start()
    {
        StartCoroutine(LaserDestruct());
    }
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        switch (_moveID)
        {
            case 0://Normal
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                break;
            case 1://Enemy backfire
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (_laserID)
        {
            case 0://Player
                if (other.tag == "Enemy" || other.tag == "Asteroid")
                {
                    Destroy(this.gameObject);
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                }
                break;
            case 1://Enemy
                if (other.tag == "Player")
                {
                    Destroy(this.gameObject);
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    Player.Instance.Damage();
                }
                else if (other.tag == "PowerUp")
                {
                    Destroy(this.gameObject);
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                }
                break;
        }
    }
    IEnumerator LaserDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
        if (this.transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
