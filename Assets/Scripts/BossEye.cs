using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossEye : MonoBehaviour
{
    [SerializeField]
    private static float _turnSpeed = 200f;
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
        if (Player.Instance != null)
        {
            Vector3 _targetLocation = Player.Instance.transform.position;
            _targetLocation.z = _myPos.z; // no 3D rotation
            Vector3 vectorToTarget = _targetLocation - _myPos;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _zRotation) * vectorToTarget;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
            Debug.DrawRay(transform.position, _targetLocation);
        }
        else
        {
            return;
        }
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
