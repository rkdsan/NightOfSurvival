using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private Color _emissionColor;
    private Color _lightColor;
    private Material emissionMaterial;

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
        _emissionColor = lampLight.color;
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
        explainComment = _isOnLight ? "LB: 끄기" : "LB: 켜기";
        emissionMaterial.SetColor("_EmissionColor", _isOnLight ? _emissionColor : Color.black);
    }

    private IEnumerator BlinkLight()
    {
        //램프마다 깜빡임 딜레이가 다르게 하기위한 offset 설정
        //WaitTimeManager에 있는 딕셔너리에 각각의 시간이 추가돼있는데
        //실수로 하면 램프개수만큼 추가되어서 0~10 정수로 지정
        yield return WaitTimeManager.WaitForSeconds(Random.Range(0, 10) * 0.1f);

        while (true)
        {
            if (_isOnLight && CheckIsInsideGhost())
            {
                lampLight.DOColor(Color.black, 0.3f)
                .SetEase(Ease.InQuart)
                .SetLoops(1, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    lampLight.color = _lightColor;
                });
            }
            yield return WaitTimeManager.WaitForSeconds(1);
        }
    }

    private bool CheckIsInsideGhost()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20, GameData.GHOST_LAYER_MASK);
        if(cols.Length > 0)
        {
            return true;
        }
        return false;

        //return false;
    }


}
