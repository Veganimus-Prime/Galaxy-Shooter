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
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _asteroidPrefab;
    private int _enemyToSpawn;
    [SerializeField][Range(0,8)]
    private int _waveCount;
    private int _enemiesInWave, _spawnedEnemies, _enemiesRemaining;
    [SerializeField]
    private List<GameObject> _enemyPrefab = new List<GameObject>();
    [SerializeField]
    private GameObject _bossEnemy;
    [SerializeField]
    private List<GameObject> _commonPowerUps = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _powerUps = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _rarePowerUps = new List<GameObject>();
    [SerializeField]
    private List<Vector3> _posToSpawn = new List<Vector3>();
    private WaitForSeconds _spawnTimer;
    private WaitForSeconds _commonPowerUpTimer;
    private WaitForSeconds _powerUpTimer;
    private WaitForSeconds _rarePowerUpTimer;
    public bool stopSpawning = true;
    public bool nextWaveReady;
    void Awake()
    {
        _instance = this;
        _spawnTimer = new WaitForSeconds(5f);
        _commonPowerUpTimer = new WaitForSeconds(8);
        _powerUpTimer = new WaitForSeconds(12);
        _rarePowerUpTimer = new WaitForSeconds(16);
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
        UIManager.Instance.waveNumberText.text = _waveCount.ToString();
        Wave(_waveCount);
        _posToSpawn[0] = new Vector3(Random.Range(-8, 8), 7, 0);
        _posToSpawn[1] = new Vector3(-10, Random.Range(-3, 3), 0);
        _posToSpawn[2] = new Vector3(10, Random.Range(-3, 3), 0);
        if (_waveCount < 9)
        {
            StartCoroutine(SpawnRoutine());
            UIManager.Instance.waveNumberText.text =  _waveCount.ToString();
        }
        else
        {
            BossSpawn();
            UIManager.Instance.waveNumberText.text = "FINAL BOSS! ";
            UIManager.Instance.bossHealthText.text = "Boss Health: 100%";
            AudioManager.Instance.BossMusic();
        }
        StartCoroutine(CommonPowerUpRoutine());
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
            _enemyToSpawn = Random.Range(0, _waveCount - 1);
            switch (_enemyToSpawn)
            {
                case 0://Basic
                    GameObject newEnemy0 = Instantiate(_enemyPrefab[0], _posToSpawn[0], Quaternion.Euler(-180, -90, 90));
                    newEnemy0.transform.parent = _enemyContainer.transform;
                    break;

                case 1://Horizontal Basic
                    GameObject newEnemy1 = Instantiate(_enemyPrefab[1], _posToSpawn[1], Quaternion.Euler(0, 0, -90));
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    break;
                case 2://Ram
                    GameObject newEnemy2 = Instantiate(_enemyPrefab[2], _posToSpawn[0], Quaternion.Euler(0,0,180));
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    break;
                case 3://Shield
                    GameObject newEnemy3 = Instantiate(_enemyPrefab[3], _posToSpawn[0], Quaternion.Euler(0, 0, 180));
                    newEnemy3.transform.parent = _enemyContainer.transform;
                    break;
                case 4://Evader
                    GameObject newEnemy4 = Instantiate(_enemyPrefab[4], _posToSpawn[1], Quaternion.Euler(0,0,-90));
                    newEnemy4.transform.parent = _enemyContainer.transform;
                    break;
                case 5://Backward fire
                    GameObject newEnemy5 = Instantiate(_enemyPrefab[5], _posToSpawn[0], Quaternion.Euler(0, 0, 180));
                    newEnemy5.transform.parent = _enemyContainer.transform;
                    break;
                case 6://Horizontal Missile
                    GameObject newEnemy6 = Instantiate(_enemyPrefab[6], _posToSpawn[2], Quaternion.Euler(0, 0, 90));
                    newEnemy6.transform.parent = _enemyContainer.transform;
                    break;
        }
    }
    void BossSpawn()
    {
        GameObject bossEnemy = Instantiate(_bossEnemy, _posToSpawn[0], Quaternion.identity);
        bossEnemy.transform.parent = _enemyContainer.transform;
    }
    IEnumerator SpawnRoutine()
    {
        while (_spawnedEnemies < _enemiesInWave)
        {
            for (int i = 0; i < _enemiesInWave; i++)
            {
                yield return _spawnTimer;
                EnemySpawn(_enemyToSpawn);
            }
        }
    }
    IEnumerator CommonPowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
            Instantiate(_commonPowerUps[(Random.Range(0, 1))], posToSpawn, Quaternion.identity);
            yield return _commonPowerUpTimer;
        }
    }
    IEnumerator PowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            yield return _powerUpTimer;
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
            Instantiate(_powerUps[(Random.Range(0,2))],posToSpawn, Quaternion.identity);
            yield return _powerUpTimer;
        }
    }
    IEnumerator RarePowerUpRoutine()
    {
        while (stopSpawning == false)
        {
            yield return _rarePowerUpTimer;
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
            Instantiate(_rarePowerUps[(Random.Range(0, 5))], posToSpawn, Quaternion.identity);
            yield return _rarePowerUpTimer;
        }
    }
}
