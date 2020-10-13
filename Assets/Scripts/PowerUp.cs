using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _speed = 2.5f;
    [SerializeField]
    private int _powerUpID;
    
    void Update()
    {
        if (Player.Instance != null)
        {
            if (Player.Instance._magnetOn == false)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, Player.Instance.transform.position, 3.0f * _speed * Time.deltaTime);
            }
        }
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.Instance.PowerUp(_powerUpID);
            Destroy(this.gameObject);
        }
    }
}
