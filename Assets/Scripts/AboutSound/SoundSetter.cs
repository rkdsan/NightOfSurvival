using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetter : MonoBehaviour
{
    public Slider slider;
    public Text valueText;
    public string keyString;

    private int value = 0;

    private void Awake()
    {
        ResetValue();
    }

    private void ResetValue()
    {
        if (PlayerPrefs.HasKey(keyString))
        {
            slider.value = PlayerPrefs.GetInt(keyString);
        }
        SetValue();
        
    }

    public void SetValue()
    {
        value = (int)slider.value;
        valueText.text = "" + value;
        PlayerPrefs.SetInt(keyString, value);
        //BGMManager.instance.SetVolume();
    }

}
