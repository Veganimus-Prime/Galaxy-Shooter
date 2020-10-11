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
    public Text scoreText;
    public Text ammoText;
    public Text thrusterText;
    public bool thrusterCharging;
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

    // Update is called once per frame
    void Start()
    {
        scoreText.text = "SCORE: " + 0;
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
            ammoText.color = Color.green;
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
    public void UpdateThrusterCharge(int thrusterCharge)
    {
        thrusterText.text = "Thrusters: " + thrusterCharge;
        if(thrusterCharge == 0)
        {
            thrusterText.color = Color.red;
        }
        else if(thrusterCharge == 100)
        {
            thrusterText.color = Color.green;
        }
        else if(thrusterCharging == true)
        {
            thrusterText.color = Color.yellow;
        }
        else
        {
            thrusterText.color = Color.white;
        }
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
