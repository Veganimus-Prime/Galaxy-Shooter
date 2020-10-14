﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null!");
            }
            return _instance;
        }
    }
    public bool _gameOver;
    void Awake()
    {
        _instance = this;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)&& _gameOver == true)
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Pause();
        }
    }
    void Pause()
    {
        if(Time.timeScale==0)
        {
            Time.timeScale = 1;
            UIManager.Instance.PauseMenu(false);
        }
        else
        {
            Time.timeScale = 0;
            UIManager.Instance.PauseMenu(true);
        }
    }
}
