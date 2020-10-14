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
        if (_triggerID == 0)
        {
            if (other.tag == "Player" || other.tag == "Laser")
            {
                _enemy.ChangeID(2);
                Destroy(this.gameObject);
            }
        }
        else if (_triggerID != 0 && other.tag == "PowerUp")
        {
            _enemy.EnemyFire();
        }
        else if (other.tag == "Player" || other.tag == "Laser")
        {
            return;
        }
    }
}
