using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Lamp : InteractiveObject
{
    //켜져있을 때, 꺼져있을 때 나오는 문자열
    public const string LIGHT_ON_STRING = "LB: 끄기";
    public const string LIGHT_OFF_STRING = "LB: 켜기";
    public static int idSetter = 0;

    public Light lampLight;
    public AudioClip lampSound;
    public LayerMask ghostLayer;
    public int id;

    private Color _lightColor;
    private Material emissionMaterial;
    private Collider[] insideGhostCols;

    private bool _isOnLight;

    //void Awake()
    //{
    //    emissionMaterial = GetComponent<MeshRenderer>().materials[3];
    //    _emissionColor = lampLight.color;
    //    _lightColor = lampLight.color;

    //    //Set Lamp 함수를 쓰면 사운드 재생돼서 직접적음
    //    SettingSave();
    //    SetLamp();
    //    StartCoroutine(BlinkLight());
    //}

    private void Start()
    {
        emissionMaterial = GetComponent<MeshRenderer>().materials[3];
        _lightColor = lampLight.color;

        SettingSave();
        SetLamp();
        StartCoroutine(BlinkLight());
    }

    private void SettingSave()
    {
        id = idSetter++;
        LampManager.instance.allLampDictionary.Add(id, this);
    }

    public override void Interact()
    {
        _isOnLight = !_isOnLight;
        SetLamp();
        SFXPlayer.instance.Play(lampSound);
    }

    public void SetLamp(bool isOnLight)
    {
        _isOnLight = isOnLight;
        SetLamp();
    }

    private void SetLamp()
    {
        lampLight.enabled = _isOnLight;
        explainComment = _isOnLight ? LIGHT_ON_STRING : LIGHT_OFF_STRING;
        emissionMaterial.SetColor("_EmissionColor", _isOnLight ? _lightColor : Color.black);
    }

    private IEnumerator BlinkLight()
    {
        //램프마다 깜빡임 딜레이를 다르게 하기위한 offset 설정
        //WaitTimeManager에 있는 딕셔너리에 각각의 시간이 추가돼있는데
        //실수(0.0f ~ 0.1f)로 하면 각각 다른 실수가 나와 딕셔너리에 많이 추가돼서 정수로 10개만 나오도록 함.
        yield return WaitTimeManager.WaitForSeconds(Random.Range(0, 10) * 0.1f);

        while (true)
        {
            float waitTime = 2f;

            if (_isOnLight && CheckIsInsideGhost())
            {
                //가까울수록 기다리는 시간 줄여서 더 깜빡이도록
                float closeGhostDis = insideGhostCols.Min(t => (transform.position - t.transform.position).sqrMagnitude);
                float subTime = 2 / (closeGhostDis * 0.1f);
                waitTime -= subTime > 1.7f ? 1.7f : subTime;

                lampLight.DOColor(Color.black, 0.4f - (0.3f/waitTime)*0.2f)
                .SetEase(Ease.InQuart)
                .SetLoops(1, LoopType.Yoyo)
                .OnUpdate(() => emissionMaterial.SetColor("_EmissionColor", lampLight.color))
                .OnComplete(() =>
                {
                    lampLight.color = _lightColor;
                    emissionMaterial.SetColor("_EmissionColor", lampLight.color);
                });
                Debug.Log("waitTime: " + waitTime);
            }
            
            yield return WaitTimeManager.WaitForSeconds(waitTime);
        }
    }

    private bool CheckIsInsideGhost()
    {
        insideGhostCols = Physics.OverlapSphere(transform.position, 20, GameData.GHOST_LAYER_MASK);


        return insideGhostCols.Length > 0;

        //return false;
    }


}
