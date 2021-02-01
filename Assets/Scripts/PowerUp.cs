using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class PowerUp : MonoBehaviour
{
    public enum PowerUpID
    {
        TripleShot = 0,
        SpeedBoost = 1,
        Shield = 2,
        IceBeam = 3,
        Life = 4,
        Ammo = 5,
        Sabotage = 6,
        LocNar = 7
    }
    public PowerUpID _powerUpID;
    private float _speed = 2.5f;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private int _scoreValue = 5;

    void Update()
    {
        if (Player.Instance != null)
        {
            if (Player.Instance.MagetOn == false)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, 3.0f * _speed * Time.deltaTime);
            }
            if(transform.position.y < -5)
            {
                Destroy(gameObject);
                UIManager.Instance.UpdateScore(_scoreValue);
            }
        }
    }
    void Damage()
    {
        _lives--;
        if(_lives==0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.Instance.PowerUp((int)_powerUpID);
            Damage();
        }
        else if (other.tag == "Enemy Laser")
        {
            Damage();
        }
    }
}
