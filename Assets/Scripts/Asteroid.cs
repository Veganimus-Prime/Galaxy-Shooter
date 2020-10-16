using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3f;
    private int _speed = 2;
    private bool _canDestroy = false;
    [SerializeField]
    private AudioSource _audio;
    public GameObject asteroidExplosion;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        transform.position = new Vector3(0, 6, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, _speed * Time.deltaTime);
        
        if (transform.position == Vector3.zero)
        {
            transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
            _canDestroy = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser" && _canDestroy == true)
        {
            _audio.Play();
            GameObject explosion = Instantiate(asteroidExplosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
           
            SpawnManager.Instance.StartSpawning();
        }
    }
}
