using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ScreenBounds : MonoBehaviour
{
    private static ScreenBounds _instance;
    public static ScreenBounds Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("ScreenBounds is NULL!");
            }
            return _instance;
        }
    }
    private Vector3 _playerPos;
    void Awake()
    {
        _instance = this;
    }
    
    void OnTriggerExit(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                _playerPos = Player.Instance.transform.position;
                if (_playerPos.x > 9.3f)
                {
                    Player.Instance.ScreenWrap(0);
                }
                else if (_playerPos.x < -9.3f)
                {
                    Player.Instance.ScreenWrap(1);
                }
                break;
        }
    }
}
