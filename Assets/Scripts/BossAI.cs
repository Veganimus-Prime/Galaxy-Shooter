using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private static BossAI _instance;
    public static BossAI Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Boss is NULL!");
            }
            return _instance;
        }
    }
    public int lives = 50;
    private float _speed = 2f;
    public Vector3 stopPos;
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        transform.position = new Vector3(0, 12, 0);
        stopPos = new Vector3(0, 4.5f, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, stopPos, _speed * Time.deltaTime);
        if (transform.position == stopPos)
        {
            return;
        }
        if(lives <=0)
        {
            lives = 0;
            Destroy(this.gameObject);
        }
    }
    void Damage(int amount)
    {
        if (lives > 0)
        {
            lives -= amount;
            UIManager.Instance.UpdateBossHealth();
        }
        else if (lives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Damage(1);
            Player.Instance.AddScore(10);
        }
        else if(other.tag == "Ice Beam")
        {
            Damage(5);
            Player.Instance.AddScore(10);
        }
    }
}
