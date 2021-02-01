using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private static float _speed = 5f;
    [SerializeField]
    private GameObject _closest;
    [SerializeField]
    private float _zRotation, _turnSpeed = 200f;
    private Vector3 playerPos;
    
    void Start()
    {
        FindClosestTarget();
    }
    void Update()
    {
        if (_closest == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 5 || transform.position.y < -5)
            {
                Destroy(this.gameObject);
            }
            else if(transform.position.x > 10 || transform.position.x < -10)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Movement();
            FindClosestTarget();
        }
    }
    void Movement()
    {
        if (_closest != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _closest.transform.position, 2f * _speed * Time.deltaTime);
            Vector3 _myPos = transform.position;
            Vector3 _targetLocation = _closest.transform.position;
            _targetLocation.z = _myPos.z; // no 3D rotation
            Vector3 vectorToTarget = _targetLocation - _myPos;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _zRotation) * vectorToTarget;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
            //Debug.DrawRay(transform.position, _targetLocation);
        }
    }
    private GameObject FindClosestTarget()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("PowerUp");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject target in targets)
        {
            Vector3 difference = target.transform.position - position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance)
            {
                _closest = target;
                distance = currentDistance;
            }
        }
        if (_closest != null)
        {
            //Debug.Log(_closest.name);

        }
        return _closest;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        if (other.tag == "Player")
        {
            Player.Instance.Damage();
        }
    }
}
