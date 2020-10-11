using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    public static CameraShake Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Camera Shake is NULL! OH NO!");
            }
            return _instance;
        }
    }
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private float _shakeDuration = 1f;
    [SerializeField]
    private float _shakeMagnitude = 0.5f;
    [SerializeField]
    private float _dampingSpeed = 1.0f;
    private Vector3 _initialPosition;
    void Awake()
    {
        _instance = this;
        if (transform == null)
        {
            _camera = GetComponent<Transform>();
        }
    }
    void Start()
    {
        _initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.localPosition = _initialPosition + Random.insideUnitSphere * _shakeMagnitude;

            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = _initialPosition;
        }
    }
    public void TriggerShake()
    {
        _shakeDuration = 0.5f;
    }
}
