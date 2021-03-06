﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);
    private Animator _anim;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _explosionClip;
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
        _audio = GetComponent<AudioSource>();
        if(_audio == null)
        {
            Debug.LogError("AudioSource is NULL!");
        }
        StartCoroutine(EnemyFireRoutine());
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed *Time.deltaTime);
        if(transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-8, 8), 5, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"|| other.tag == "Laser")
        {
            _anim.SetTrigger("OnEnemyDeath");
            _audio.PlayOneShot(_explosionClip);
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.3f);
            Player.Instance.AddScore(10);
            if (other.tag == "Player")
            {
                Player.Instance.Damage();
            }
        }
    }
    IEnumerator EnemyFireRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3));
        Instantiate(_enemyLaser, transform.position - _laserOffset, Quaternion.identity);
    }
}
