using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
            if (this.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            if (this.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Player.Instance.Damage();
        }
        else if (other.tag == "PowerUp")
        {
            Destroy(this.gameObject);
        }
    }
}
