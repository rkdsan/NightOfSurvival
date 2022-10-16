using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    public AudioClip EndingBGM;

    private void Awake()
    {
        BGMPlayer.instance.SetAudioClip(EndingBGM);
        BGMPlayer.instance.source.Play();
    }

}
