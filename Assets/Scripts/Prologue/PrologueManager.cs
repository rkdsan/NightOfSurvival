using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{
    public List<CutScene> cuts;

    private int cutIdx = 0;

    private void OnEnable()
    {
        cutIdx = 0;
        foreach(var cut in cuts)
        {
            cut.gameObject.SetActive(false);
        }
        cuts[0].gameObject.SetActive(true);
        LoadNextCut();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextCut();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void LoadNextCut()
    {
        if (cuts[cutIdx].isLast)
        {
            if (cuts.Count == cutIdx + 1)
            {
                TitleSceneManager.instance.LoadNextScene();
                return;
            }
            cuts[cutIdx].gameObject.SetActive(false);
            cuts[++cutIdx].gameObject.SetActive(true);

        }
        cuts[cutIdx].TurnOnNextCut();
    }


}
