using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private int _laserID;

    void Update()
    {
        if (this.transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        switch (_laserID)
        {
            case 0://Player
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                if (transform.position.y > 5)
                {
                    Destroy(this.gameObject);
                }
                break;
            case 1://Enemy
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y < -5)
                {
                    Destroy(this.gameObject);
                }
                break;
            case 2://EnemyX
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y > 9)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (_laserID)
        {
            case 0://Player
                switch (Player.Instance._fireMode)
                {
                    case 0://Normal
                        if (other.tag == "Enemy" || other.tag == "Asteroid")
                        {
                            Destroy(this.gameObject);
                        }
                        break;
                    case 1://TripleShot
                        if (other.tag == "Enemy")
                        {
                            Destroy(this.gameObject);
                        }
                        break;
                    case 2://IceBeam
                        if (other.tag == "Enemy")
                        {
                            Destroy(this.gameObject);
                        }
                        break;
                }
                break;
            case 1://Enemy
                if (other.tag == "Player")
                {
                    Destroy(this.gameObject);
                    Player.Instance.Damage();
                }
                else if (other.tag == "PowerUp")
                {
                    Destroy(this.gameObject);
                }
                break;
            case 2://EnemyX
                if (other.tag == "Player")
                {
                    Destroy(this.gameObject);
                    Player.Instance.Damage();
                }
                else if (other.tag == "PowerUp")
                {
                    Destroy(this.gameObject);
                }
                break;
        }


    }
}
