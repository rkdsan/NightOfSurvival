using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    public static LampManager instance;

    public Dictionary<int, Lamp> allLampDictionary;

    private void Awake()
    {
        if(instance != null)
        { 
            Destroy(gameObject);
        }
        instance = this;
        Lamp.idSetter = 0;
        allLampDictionary = new Dictionary<int, Lamp>();
    }
}
