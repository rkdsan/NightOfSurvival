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
        DontDestroyOnLoad(gameObject);
        StartCoroutine(SetSource());
    }

    private IEnumerator SetSource()
    {
        targetVolume = source.volume;
        source.volume = 0;
        yield return WaitTimeManager.WaitForSeconds(1);

        source.Play();
        int i = 20;
        while (i-- > 0)
        {
            source.volume += targetVolume / 20;
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        source.volume = targetVolume;
    }

    public void SetVolume()
    {
        source.volume = PlayerPrefs.GetInt("BGMVolume", 30) * 0.01f;
    }
}
