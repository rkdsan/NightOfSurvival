using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
    public static EffectSoundManager instance;
    public AudioSource effectSound_doorSound;
    public AudioSource effectSound_lampOff;
    public AudioSource effectSound_lampOn;
    public AudioSource effectSound_pickUp;
    public AudioSource effectSound_boom;
    public AudioSource effectSound_chop;

    private void Awake()
    {
        instance = this;
    }


}
