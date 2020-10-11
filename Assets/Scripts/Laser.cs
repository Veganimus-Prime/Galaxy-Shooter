using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(transform.position.y > 5)
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
      switch(Player.Instance._fireMode)
      {
            case 0://Normal
                if (other.tag == "Enemy" || other.tag == "Asteroid")
                {
                    Destroy(this.gameObject);
                }
                    break;
            case 1://TripleShot
                if(other.tag == "Enemy")
                {
                    Destroy(this.gameObject);
                    if (this.transform.parent != null)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
                break;
            case 2://IceBeam
                if(other.tag == "Enemy")
                {
                    Destroy(this.gameObject);
                }
                break;

      }
            
        
    }
}
