using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeMeter : MonoBehaviour
{
    private static GaugeMeter _instance;
    public static GaugeMeter Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GaugeMeter is NULL!");
            }
            return _instance;
        }
    }
    private RectTransform _rt;
    
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void UpdateGauge(float length)
    {
        _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, length);
    }
}
