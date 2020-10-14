using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private int _lives = 3;
    private Enemy _enemy;
    [SerializeField]
    private GameObject _closest;
   
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        if(_enemy == null)
        {
            Debug.LogError("Enemy is Null!");
        }
        else
        {
            FindClosestTarget();
        }
    }
    void Update()
    {
        if (_closest == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 5)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _closest.transform.position, step);
            FindClosestTarget();
        }
        
    }
    private GameObject FindClosestTarget()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject target in targets)
        {
            Vector3 difference = target.transform.position - position;
            float currentDistance = difference.sqrMagnitude;
            if(currentDistance < distance)
            {
                _closest = target;
                distance = currentDistance;
            }
        }
        if (_closest != null)
        {
            Debug.Log(_closest.name);
           
        }
        return _closest;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _enemy.Damage();
            Damage();
            Player.Instance.AuxillaryRecharge();
        }
    }
    void Damage()
    {
        _lives--;
        if(_lives==0)
        {
            Destroy(this.gameObject);
        }
    }
}
