using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LampBlinker : MonoBehaviour
{
    public Light lampLight;

    private List<float> waitTimeList;
    private Material _emissionMaterial;

    private Color originLightColor;
    private float waitTime = 3f;

    void Start()
    {
        originLightColor = lampLight.color;
        _emissionMaterial = GetComponent<MeshRenderer>().materials[3];
        waitTimeList = new List<float>() { 0.9f, 1f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.5f, 2.6f
            , 2.3f, 2.4f, 2.5f, 2.6f, 2.8f, 2.9f, 2.7f, 2.7f};
        StartCoroutine(BlinkLight());
    }

    private IEnumerator BlinkLight()
    {
        int counter = 0;
        while (true)
        {
            counter++;
            waitTime = waitTimeList[Random.Range(0, waitTimeList.Count)];
            ExecuteDOColor();
            _emissionMaterial.SetColor("_EmissionColor", lampLight.color);
            yield return WaitTimeManager.WaitForSeconds(waitTime);

            if ((waitTime > 1f && counter < 1) || counter < 4)
                continue;

            counter = 0;
            waitTime = 0.2f;
            int repeat = Random.Range(4, 8);
            for (int i = 0; i < repeat; i++)
            {
                ExecuteDOColor();
                yield return WaitTimeManager.WaitForSeconds(waitTime);
            }

        }
    }

    private void ExecuteDOColor()
    {
        float fadeTime = waitTime * 0.8f;
        lampLight.DOColor(Color.black, fadeTime > 0.4f ? 0.4f : fadeTime)
        .SetEase(Ease.InQuart)
        .SetLoops(1, LoopType.Yoyo)
        .OnUpdate(() => SetEmissionFromLightColor())
        .OnComplete(() =>
        {
            lampLight.color = originLightColor;
            SetEmissionFromLightColor();
        });
    }

    private void SetEmissionFromLightColor() => _emissionMaterial.SetColor("_EmissionColor", lampLight.color);
}
