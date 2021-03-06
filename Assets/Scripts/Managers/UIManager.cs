﻿using System.Collections;
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
