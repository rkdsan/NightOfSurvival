using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public AudioSource source;

    private float targetVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        source.volume = PlayerPrefs.GetInt("BGMVolume", 30) * 0.01f;
        source.Play();
        DontDestroyOnLoad(gameObject);
    }


    public void SetVolume()
    {
        source.volume = PlayerPrefs.GetInt("BGMVolume", 30) * 0.01f;
    }
}
