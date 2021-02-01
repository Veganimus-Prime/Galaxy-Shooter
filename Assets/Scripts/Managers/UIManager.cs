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
    [SerializeField]
    private GameObject _staticCanvas;
    [SerializeField]
    private GameObject _activeCanvas;
    public GameObject pauseMenu;
    public GameObject waveCompleteText, nextWaveText;
    public Text scoreText;
    public Text ammoText;
    public Text reserveAmmoText;
    public Text auxillaryText;
    public Text waveNumberText;
    public Text bossHealthText;
    public bool auxillaryCharging;
    [SerializeField]
    private Graphic gauge;
    [SerializeField]
    private Image _livesImg;
    public static int currentScore;
    [SerializeField]
    private List<Sprite> _livesSprite = new List<Sprite>();
    [SerializeField]
    private GameObject _gameOverText, _restartText;
    [SerializeField]
    private Animator _scoreAnim;
    [SerializeField]
    private Animator _ammoAnim;
    public static int playerAmmo;
    private WaitForSeconds _flickerTime;

    void Awake()
    {
        _instance = this;
        _flickerTime = new WaitForSeconds(0.75f);
        
    }
    void Start()
    {
        _activeCanvas.SetActive(true);
        scoreText.text = "0000";
        bossHealthText.text = "";
        gauge = GameObject.Find("Gauge_meter").GetComponentInChildren<Graphic>();
        gauge.color = new Color32(0, 150, 0, 255);
       /* _scoreAnim = GameObject.Find("Score_text").GetComponentInChildren<Animator>();
            if(_scoreAnim == null)
            {
            Debug.LogError("Score Animator is null");
            }
        _ammoAnim = GameObject.Find("Ammo_Count_text").GetComponentInChildren<Animator>();
        if (_ammoAnim == null)
        {
            Debug.LogError("Ammo Animator is null");
        }*/

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
        currentScore += points;
        scoreText.text = currentScore.ToString("000#");
        //_scoreAnim.SetTrigger("AddScore");
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
        playerAmmo = ammoCount;
        ammoText.text = playerAmmo.ToString();
        //_ammoAnim.SetInteger("Ammo", playerAmmo);
        if(playerAmmo == 0)
        {
            ammoText.text = "RELOAD!";
            //_ammoAnim.SetTrigger("Reload");
        }
    }
    public void UpdateReserveAmmo(int reserve)
    {
        reserveAmmoText.text = reserve.ToString();
        if (reserve == 0)
        {
            reserveAmmoText.color = Color.red;
        }
        else
        {
            reserveAmmoText.color = Color.white;
        }
    }
    public void UpdateAuxillaryCharge(float auxillaryCharge)
    {
        auxillaryText.text = "Power: " + auxillaryCharge.ToString()+ "%";
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
        bossHealthText.text = "Boss Health: " + BossAI.Instance.BossLives *2 + "%";
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
            yield return _flickerTime;
            _gameOverText.SetActive(false);
            yield return _flickerTime;
        }
    }
}
