using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneLoader : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameData.PLAYER_LAYER)
        {
            SFXPlayer.instance.StopAllSFX();
            LoadingSceneManager.LoadScene("EndingScene");
        };

    }

}
