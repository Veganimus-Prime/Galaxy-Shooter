using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamTrigger : MonoBehaviour
{
    private Enemy _enemy;
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player"|| other.tag == "Laser")
        {
            _enemy.ChangeID(2);
            Destroy(this.gameObject);
        }
        else if (other.tag == "PowerUp")
        {
            _enemy.EnemyFire();
        }
    }
}
