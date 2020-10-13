using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private static Shield _instance;
    public static Shield Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Shield is NULL!");
            }
            return _instance;
        }
    }
    public int shieldHP = 3;
    private SpriteRenderer _sprite;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        if (_sprite == null)
        {
            Debug.LogError("Sprite Renderer is NULL!");
        }
    }
    public void ShieldHit()
    {
        if (shieldHP <= 0)
        {
            shieldHP = 0;
        }
        else
        {
            shieldHP--;
            switch (shieldHP)
            {
                case 0:
                    Player.Instance.isShieldActive = false;
                    Player.Instance.powerUpShield = false;
                    Player.Instance._shield.SetActive(false);
                    break;
                case 1:
                    transform.localScale = new Vector3(1.50f, 1.5f, 1.5f);
                    break;
                case 2:
                    transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                    break;
                case 3:
                    transform.localScale = new Vector3(2f, 2f, 2f);
                    break;
            }
        }
        
    }
}
