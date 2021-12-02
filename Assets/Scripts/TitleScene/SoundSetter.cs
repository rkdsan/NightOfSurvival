using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetter : MonoBehaviour
{
    public Slider slider;
    public Text valueText;

    private int value = 0;

    private void Update()
    {
        SetValue();
    }

    private void SetValue()
    {
        value = (int)(100 * slider.value);
        valueText.text = "" + value;
    }

}
