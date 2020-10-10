using System.Collections;
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
    private GameObject _enemyPrefab, _enemyContainer;

    public bool stopSpawning = true;
    [SerializeField]
    private GameObject[] _powerUps = new GameObject[3];
    
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        
    }
    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
    public void StartSpawning()
    {
        stopSpawning = false;
        StartCoroutine(SpawnRoutine());
        StartCoroutine(PowerUpRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 6, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator PowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 6, 0);
            Instantiate(_powerUps[(Random.Range(0,3))], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 7));
        }
    }
}
