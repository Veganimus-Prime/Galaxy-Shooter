using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance ==null)
            {
                Debug.LogError("UIManager is NULL!");
            }
            return _instance;
        }
    }
    public GameObject pauseMenu;
    public GameObject waveCompleteText, nextWaveText;
    public Text scoreText;
    public Text ammoText;
    public Text auxillaryText;
    public Text waveNumberText;
    public Text bossHealthText;
    public bool auxillaryCharging;
    [SerializeField]
    private Graphic gauge;
    [SerializeField]
    private Image _livesImg;
    private int _currentScore;
    [SerializeField]
    private Sprite[] _livesSprite = new Sprite[4];
    [SerializeField]
    private GameObject _gameOverText, _restartText;
    
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        scoreText.text = "SCORE: " + 0;
        bossHealthText.text = "";
        gauge = GameObject.Find("Gauge_meter").GetComponent<Graphic>();
        gauge.color = new Color32(0, 150, 0, 255);
    }
    public void WaveComplete()
    {
        if (SpawnManager.Instance.nextWaveReady == true)
        {
            waveCompleteText.SetActive(true);
            nextWaveText.SetActive(true);
        }
    }
    public void UpdateScore(int points)
    {
        _currentScore = points;
        scoreText.text = "SCORE: " + _currentScore;
    }
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];
        if (currentLives == 0)
        {
            _restartText.SetActive(true); 
            GameManager.Instance._gameOver = true;
            StartCoroutine(GameOverFlicker());
        }
    }
    public void UpdateAmmoCount(int ammoCount)
    {
        ammoText.text = "Ammo: " + ammoCount;
        if(ammoCount == 0)
        {
            ammoText.color = Color.red;
        }
        else if(ammoCount >=15)
        {
            ammoText.text = "Ammo: MAX";
            ammoText.color = new Color32(0, 150, 0,255);
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
    public void UpdateAuxillaryCharge(float auxillaryCharge)
    {
        auxillaryText.text = "Aux. Power: " + auxillaryCharge + "%";
         if(auxillaryCharge == 100)
        {
           gauge.color = new Color32(0,150,0,255);
        }
        else if(auxillaryCharging == true)
        {
            gauge.color = Color.grey;
        }
        
    }
    public void UpdateBossHealth()
    {
        bossHealthText.text = "Boss Health: " + BossAI.Instance.lives *2 + "%";
    }
    public void PauseMenu(bool paused)
    {
        pauseMenu.SetActive(paused);
    }
   public IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
