using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer instance;
    public List<AudioSource> sfxPlayers;

    private int playerIdx;
    private float sfxVolume;
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
        playerIdx = 0;
        SetVolume();
        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume()
    {
        sfxVolume = PlayerPrefs.GetInt(SFXVolumeSetter.keyString, 30) * 0.01f;
        foreach(AudioSource sfxplayer in sfxPlayers)
        {
            sfxplayer.volume = sfxVolume;
        }
    }

    public AudioSource Play(AudioClip clip)
    {
        AudioSource nowPlayer = null;

        foreach (AudioSource sfxplayer in sfxPlayers)
        {
            if (!sfxplayer.isPlaying)
            {
                nowPlayer = sfxplayer;
                break;
            }
        }

        if (nowPlayer == null)
        {
            nowPlayer = AddNewPlayer();
        }

        nowPlayer.clip = clip;
        nowPlayer.Play();

        return nowPlayer;
    }

    private AudioSource AddNewPlayer()
    {
        AudioSource newPlayer;
        newPlayer = gameObject.AddComponent<AudioSource>();
        newPlayer.playOnAwake = false;
        newPlayer.volume = sfxVolume;
        playerIdx++;
        sfxPlayers.Add(newPlayer);
        return newPlayer;
    }

}
