﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SpawnManager is NULL!");
            }
            return _instance;
        }
    }
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _asteroidPrefab;
    private int _enemyToSpawn;
    private int _waveCount;
    private int _enemiesInWave, _spawnedEnemies, _enemiesRemaining;
    [SerializeField]
    private GameObject[] _enemyPrefab = new GameObject[3];
    [SerializeField]
    private GameObject[] _powerUps = new GameObject[3];
    [SerializeField]
    private GameObject[] _rarePowerUps = new GameObject[5];
    private Vector3[] _posToSpawn = new Vector3[2];
    
    public bool stopSpawning = true;
    public bool nextWaveReady;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _enemiesInWave = 1;
        nextWaveReady = false;
    }
    void Update()
    {
        if (_spawnedEnemies == _enemiesInWave)
        {
            stopSpawning = true;
            StopAllCoroutines();
            if (_enemiesRemaining == 0)
            {
                nextWaveReady = true;
                UIManager.Instance.WaveComplete();
            }
        }
    }
    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
    public void StartSpawning()
    {
        stopSpawning = false;
        nextWaveReady = false;
        _spawnedEnemies = 0;
        _waveCount++;
        UIManager.Instance.waveNumberText.text = "Wave: " + _waveCount;
        Wave(_waveCount);
        _posToSpawn[0]= new Vector3(Random.Range(-8, 8), 7, 0);
        _posToSpawn[1] = new Vector3(-10, Random.Range(-3, 3), 0);

        StartCoroutine(SpawnRoutine());
        StartCoroutine(PowerUpRoutine());
        StartCoroutine(RarePowerUpRoutine());
    }
    void Wave(int _waveCount)
    {
        _enemiesInWave = _waveCount * 3;
        _enemiesRemaining = _enemiesInWave;
    }
    public void EnemyKilled()
    {
        _enemiesRemaining--;
    }
    public void EnemySpawn(int _enemyToSpawn)
    {
            _spawnedEnemies++;
            //_enemyToSpawn = Random.Range(0, 4);
            switch (_waveCount)
            {
                case 1:
                    _enemyToSpawn = 0;
                    break;
                case 2:
                    _enemyToSpawn = Random.Range(0, 1);
                    break;
                case 3:
                    _enemyToSpawn = Random.Range(0, 2);
                    break;
                case 4:
                    _enemyToSpawn = Random.Range(0, 3);
                    break;
                case 5:
                    _enemyToSpawn = Random.Range(0, 4);
                    break;
            }
            switch (_enemyToSpawn)
            {
                case 0:
                    GameObject newEnemy0 = Instantiate(_enemyPrefab[0], _posToSpawn[0], Quaternion.identity);
                    newEnemy0.transform.parent = _enemyContainer.transform;
                    break;

                case 1:
                    GameObject newEnemy1 = Instantiate(_enemyPrefab[1], _posToSpawn[1], Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    break;
                case 2:
                    GameObject newEnemy2 = Instantiate(_enemyPrefab[2], _posToSpawn[0], Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    break;
                case 3:
                    GameObject newEnemy3 = Instantiate(_enemyPrefab[3], _posToSpawn[0], Quaternion.identity);
                    newEnemy3.transform.parent = _enemyContainer.transform;
                    break;
            }
    }
    IEnumerator SpawnRoutine()
    {
        while (_spawnedEnemies < _enemiesInWave)
        {
            for (int i = 0; i < _enemiesInWave; i++)
            {
                yield return new WaitForSeconds(5f);
                EnemySpawn(_enemyToSpawn);
                //yield return new WaitForSeconds(5f);
            }
        }
    }
    IEnumerator PowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
            Instantiate(_powerUps[(Random.Range(0,3))],posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7, 10));
        }
    }
    IEnumerator RarePowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            yield return new WaitForSeconds(10f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
            Instantiate(_rarePowerUps[(Random.Range(0, 5))], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15, 20));
        }
    }
}
