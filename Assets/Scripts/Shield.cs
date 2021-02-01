using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
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
    void Awake()
    {
        _instance = this;
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
                    Player.Instance.IsShieldActive = false;
                    Player.Instance.PowerUpShield = false;
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
