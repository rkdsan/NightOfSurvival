using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookSensitivitySetter : MonoBehaviour
{
    public const string MOUSE_SENSE_KEY = "MouseSense";
    public const int DEFAULT_MOUSE_SENSE = 35;

    public Slider slider;
    public Text valueText;

    private int value = 50;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(MOUSE_SENSE_KEY))
        {
            slider.value = PlayerPrefs.GetInt(MOUSE_SENSE_KEY);
        }
        else
        {
            slider.value = DEFAULT_MOUSE_SENSE;
        }
        SetMouseSense();
    }


    public void SetMouseSense()
    {
        value = (int)slider.value;
        valueText.text = "" + value;
        PlayerPrefs.SetInt(MOUSE_SENSE_KEY, value);
        GameManager.instance.playerController.SetMouseSense(value);
    }

}
