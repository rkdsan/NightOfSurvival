using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public Image fillImage;

    private static string nextSceneName;

    public static void LoatScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {

            yield return null;

            if(op.progress < 0.9f)
            {
                Debug.Log("·Îµù");
                fillImage.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                fillImage.fillAmount = Mathf.Lerp(0.9f, 1, timer);
                if(fillImage.fillAmount >= 1)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }
}
