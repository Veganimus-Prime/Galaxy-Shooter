using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class Laser : MonoBehaviour
{
    private enum LaserID
    {
        Player, Enemy
    }
    [SerializeField]
    private LaserID _laserID;
    private enum MoveID
    {
        Normal, Backfire
    }
    [SerializeField]
    private MoveID _moveID;
    [SerializeField]
    private static float _speed = 8f;
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
        switch ((int)_moveID)
        {
            case 0://Normal
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                break;
            case 1://Enemy backfire
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        switch ((int)_laserID)
        {
            case 0://Player
                if (other.tag == "Enemy" || other.tag == "Asteroid")
                {
                    LaserExplode();
                }
                break;
            case 1://Enemy
                if (other.tag == "Player")
                {
                    LaserExplode();
                    Player.Instance.Damage();
                }
                else if (other.tag == "PowerUp")
                {
                    LaserExplode();
                }
                break;
        }
    }
    public void LaserExplode()
    {
        Destroy(gameObject);
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }
   
    IEnumerator LaserDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
