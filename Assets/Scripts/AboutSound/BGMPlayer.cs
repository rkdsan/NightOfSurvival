using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer instance;
    public AudioSource source;


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
        source.volume = PlayerPrefs.GetInt(BGMVolumeSetter.keyString, 30) * 0.01f;
    }

    public void SetAudioClip(AudioClip clip)
    {
        source.clip = clip;
    }
}
