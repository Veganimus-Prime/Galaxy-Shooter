using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Wave Manager is Null!");
            }
            return _instance;
        }
    }
    [SerializeField]
    private int _waveCount = 0;
    [SerializeField]
    private int _enemiesInWave, _enemiesRemaining = 1, _enemyType;
    [SerializeField]
    private bool _waveComplete = false;
    
    void Start()
    {
        
    }
    void Update()
    {
        if(_enemiesRemaining == 0)
        {
            _waveComplete = true;
            WaveIncrease();
        }
        if(_waveCount > 5)
        {
            _waveCount = 5;
        }
    }
    void Wave(int _waveCount)
    {
        _enemiesRemaining = _enemiesInWave;
            switch(_waveCount)
            {
                case 1:
                    _enemiesInWave = 5;
                    _enemyType = 0;
                    break;
                case 2:
                    _enemiesInWave = 8;
                    _enemyType = Random.Range(0, 1);
                    break;
                case 3:
                    _enemiesInWave = 12;
                    _enemyType = Random.Range(0, 2);
                    break;
                case 4:
                    _enemiesInWave = 15;
                    _enemyType = Random.Range(0, 3);
                    break;
                case 5:
                    _enemiesInWave = 20;
                    _enemyType = Random.Range(0, 4);
                    break;
            }
        
    }
    void WaveIncrease()
    {
        _waveComplete = false;
        _waveCount++;
        Wave(_waveCount);
    }
    public void EnemyKilled()
    {
        _enemiesRemaining--;
    }
    IEnumerator WaveActiveRoutine()
    {
        SpawnManager.Instance.EnemySpawn(_enemyType);
        yield return new WaitForSeconds(5f);
    }
}
