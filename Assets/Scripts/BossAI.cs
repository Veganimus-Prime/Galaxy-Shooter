using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
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
    public int BossLives { get; private set; } = 50;
    private int ScoreValue { get; } = 20;
    private float Speed { get; } = 2f;
    private Vector3 StopPos { get; set; }
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        transform.position = new Vector3(0, 12, 0);
        StopPos = new Vector3(0, 4.5f, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, StopPos, Speed * Time.deltaTime);
        if (transform.position == StopPos)
        {
            return;
        }
        if(BossLives <=0)
        {
            BossLives = 0;
            Destroy(this.gameObject);
        }
    }
    void Damage(int amount)
    {
        if (BossLives > 0)
        {
            BossLives -= amount;
            UIManager.Instance.UpdateBossHealth();
        }
        else if (BossLives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            Damage(1);
            UIManager.Instance.UpdateScore(ScoreValue);
        }
        else if(other.tag == "Ice Beam")
        {
            Damage(5);
            UIManager.Instance.UpdateScore(ScoreValue);
        }
    }
}
