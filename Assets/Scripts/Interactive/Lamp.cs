using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lamp : InteractiveObject
{
    public Light lampLight;
    public AudioClip lampSound;
    public LayerMask ghostLayer;

    private Color _emissionColor;
    private Color _lightColor;
    private Ray forFindGhostRay;
    private Material emissionMaterial;

    private bool _isOnLight;
    private bool _isInGhost;

    void Awake()
    {
        emissionMaterial = GetComponent<MeshRenderer>().materials[3];
        _emissionColor = lampLight.color;
        _lightColor = lampLight.color;

        lampLight.enabled = _isOnLight;
        explainComment = _isOnLight ? "LB: ²ô±â" : "LB: ÄÑ±â";
        emissionMaterial.SetColor("_EmissionColor", _isOnLight ? _emissionColor : Color.black);

        forFindGhostRay.direction = Vector3.zero;
        forFindGhostRay.origin = transform.position;

        StartCoroutine(BlinkLight());
    }

    public override void Interact()
    {
        _isOnLight = !_isOnLight;
        SetLamp();
    }

    private void SetLamp()
    {
        lampLight.enabled = _isOnLight;
        explainComment = _isOnLight ? "LB: ²ô±â" : "LB: ÄÑ±â";
        emissionMaterial.SetColor("_EmissionColor", _isOnLight ? _emissionColor : Color.black);
        SFXPlayer.instance.Play(lampSound);
        //if (_isOnLight)
        //    lampLight.DOColor(Color.black, 0.3f)
        //        .SetEase(Ease.InQuart)
        //        .SetLoops(5, LoopType.Yoyo)
        //        .OnComplete(() =>
        //        {
        //            lampLight.color = _lightColor;
        //        });
    }

    private IEnumerator BlinkLight()
    {
        while (true)
        {
            if (_isOnLight && CheckIsInsideGhost())
            {
                lampLight.DOColor(Color.black, 0.3f)
                .SetEase(Ease.InQuart)
                .SetLoops(5, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    lampLight.color = _lightColor;
                });
            }
            yield return WaitTimeManager.WaitForSeconds(2);
        }
    }

    private bool CheckIsInsideGhost()
    {
        //Vector3 dir;
        //foreach (Ghost ghost in Ghost.allGhostList)
        //{
        //    dir = transform.position - ghost.transform.position;
        //    if(dir.magnitude < 10)
        //    {
        //        return true;
        //    }
        //}

        //RaycastHit hit;
        //bool flag = Physics.SphereCast(transform.position, 10, Vector3.left,
        //    out hit, 0.1f, GameData.GHOST_LAYER);

        //Debug.Log("°á°ú: " + flag);
        //return flag;

        Collider[] cols = Physics.OverlapSphere(transform.position, 10, GameData.GHOST_LAYER);
        if(cols.Length > 0)
        {
            return true;
        }
        return false;

        //return false;
    }

}
