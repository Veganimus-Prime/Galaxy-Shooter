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
    private GameObject[] _enemyPrefab = new GameObject[3];
    [SerializeField]
    private GameObject _enemyContainer;
    private int _enemyToSpawn;

    public bool stopSpawning = true;
    [SerializeField]
    private GameObject[] _powerUps = new GameObject[3];
    [SerializeField]
    private GameObject[] _rarePowerUps = new GameObject[4];
    private Vector3[] _posToSpawn = new Vector3[2];
    void Awake()
    {
        _instance = this;
    }
    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
    public void StartSpawning()
    {
        stopSpawning = false;
        _posToSpawn[0]= new Vector3(Random.Range(-8, 8), 7, 0);
        _posToSpawn[1] = new Vector3(-10, Random.Range(-3, 3), 0);

        StartCoroutine(SpawnRoutine());
        StartCoroutine(PowerUpRoutine());
        StartCoroutine(RarePowerUpRoutine());
    }
    
    IEnumerator SpawnRoutine()
    {
        while (stopSpawning == false)
        {
            _enemyToSpawn = Random.Range(0, 4);
            switch (_enemyToSpawn)
            {
                case 0:
                    GameObject newEnemy0 = Instantiate(_enemyPrefab[0], _posToSpawn[0], Quaternion.identity);
                    newEnemy0.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(5f);
                    break;

                case 1:
                    GameObject newEnemy1 = Instantiate(_enemyPrefab[1], _posToSpawn[1], Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(5f);
                    break;
                case 2:
                    GameObject newEnemy2 = Instantiate(_enemyPrefab[2], _posToSpawn[0], Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(5f);
                    break;
                case 3:
                    GameObject newEnemy3 = Instantiate(_enemyPrefab[3], _posToSpawn[0], Quaternion.identity);
                    newEnemy3.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(5f);
                    break;

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
            Instantiate(_rarePowerUps[(Random.Range(0, 4))], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15, 20));
        }
    }
}
