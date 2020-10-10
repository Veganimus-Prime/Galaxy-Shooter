using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3f;
    public GameObject asteroidExplosion;
    
    void Update()
    {
       transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            GameObject explosion = Instantiate(asteroidExplosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
           
            SpawnManager.Instance.StartSpawning();
        }
    }
}
