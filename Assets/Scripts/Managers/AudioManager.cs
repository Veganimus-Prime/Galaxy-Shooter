using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AudioManager is NULL!");
            }
            return _instance;
        }
    }
    public AudioClip bossMusic;
    private AudioSource _audio;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.Play();
    }

    // Update is called once per frame
    public void BossMusic()
    {
        _audio.clip = bossMusic;
        _audio.Play();
    }

}
