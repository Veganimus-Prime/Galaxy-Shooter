using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamTrigger : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField]
    private int _triggerID;
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (_triggerID)
        {
            case 0://ram
                if (other.tag == "Player" || other.tag == "Laser")
                {
                    _enemy.thrusters.SetActive(true);
                    Destroy(this.gameObject);
                }
                break;
            case 1:
                if (other.tag == "PowerUp")
                {
                    _enemy.EnemyFire();
                }
                break;
            case 2://proximity
                if (other.tag == "Laser")
                {
                    //_enemy.Speed = 8;
                    _enemy.thrusters.SetActive(true);
                }
                break;
            default:
                return;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (_triggerID == 2 && other.tag == "Laser")
        {
            //_enemy.Speed = 4;
            _enemy.thrusters.SetActive(false);
        }
    }
}
