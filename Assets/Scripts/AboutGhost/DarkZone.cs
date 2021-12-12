using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkZone : MonoBehaviour
{
    public Lamp[] lamps;
    public LayerMask playerLayer;
    public ShadowGhost shadowGhost;

    private Coroutine Co_CheckInLamp;
    private Collider[] colls;
    private int inDarkTime;
    private bool inPlayer;

    void Start()
    {
        inDarkTime = 0;
        inPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //inPlayer는 코루틴 중복실행 방지
        if (other.CompareTag("Player") && !inPlayer)
        {
            inDarkTime = 0;
            Co_CheckInLamp = StartCoroutine(CheckInLamp());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && Co_CheckInLamp != null)
        {
            StopCoroutine(Co_CheckInLamp);
            inPlayer = false;
        }
    }

    public IEnumerator CheckInLamp()
    {
        inPlayer = true;
        TutorialManager.instance.ShowTutoWindow
            ("그슨대의 구역에 진입하였습니다.\n어둠 속을 벗어나세요.");

        while (true)
        {
            foreach (Lamp lamp in lamps)
            {
                if (lamp.lampLight.enabled)
                {
                    Debug.Log("Length: " + colls.Length);
                    if (colls.Length > 0)
                    {
                        inDarkTime = -1;
                        break;
                    }
                }
            }

            inDarkTime++;
            //Debug.Log("어둠속에 있던 시간:" + inDarkTime);
            if (CheckOverTime()) break;
            yield return WaitTimeManager.WaitForSeconds(1);
        }
    }

    public bool CheckOverTime()
    {
        if(inDarkTime > 10)
        {
            shadowGhost.StartKillPlayer();
            return true;
        }
        return false;
    }


}
