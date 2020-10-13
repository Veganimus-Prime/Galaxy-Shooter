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
    public Text auxillaryText;
    public bool auxillaryCharging;
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
    public void UpdateAuxillaryCharge(int auxillaryCharge)
    {
        auxillaryText.text = "Aux. Power: " + auxillaryCharge;
        if(auxillaryCharge == 0)
        {
            auxillaryText.color = Color.red;
        }
        else if(auxillaryCharge == 100)
        {
            auxillaryText.color = Color.green;
        }
        else if(auxillaryCharging == true)
        {
            auxillaryText.color = Color.yellow;
        }
        else
        {
            auxillaryText.color = Color.white;
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
