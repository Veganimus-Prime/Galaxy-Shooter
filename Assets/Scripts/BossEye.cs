using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour
{
    [SerializeField]
    private float _turnSpeed = 200f;
    private Player _closest;
    [SerializeField]
    private float _zRotation;
    [SerializeField]
    private bool isPaired=false;
    [SerializeField]
    private GameObject _pairedObj;
    [SerializeField]
    private GameObject _bossProjectile;
    private Vector3 _myPos;
    private SpriteRenderer _sprite;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
       // FindClosestTarget();
        if(isPaired == true)
        {
            _myPos = _pairedObj.transform.position;
        }
        else
        {
            _myPos = transform.position;
        }
        StartCoroutine(BossFireRoutine());
    }
    void Update()
    {
        TurretRotation();
    }
    private void TurretRotation()
    {
        //Vector3 myLocation = transform.position;
        if (Player.Instance != null)
        {
            Vector3 targetLocation = Player.Instance.transform.position;
            targetLocation.z = _myPos.z; // no 3D rotation
            Vector3 vectorToTarget = targetLocation - _myPos;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _zRotation) * vectorToTarget;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
            Debug.DrawRay(transform.position, targetLocation);
        }
        else
        {
            return;
        }
    }
    private Player FindClosestTarget()
    {
        Player[] targets;
        targets = FindObjectsOfType<Player>();
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Player target in targets)
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
            Debug.Log(_closest.name);

        }
        return _closest;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"&& isPaired==true)
        {
            _myPos = this.transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && isPaired == true)
        {
            _myPos = _pairedObj.transform.position;
        }
    }
    IEnumerator BossFireRoutine()
    {
        while (Player.Instance != null)
        {
            yield return new WaitForSeconds(3f);
            _sprite.color = Color.red;
            Instantiate(_bossProjectile, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
            _sprite.color = Color.white;
        }
    }
}
