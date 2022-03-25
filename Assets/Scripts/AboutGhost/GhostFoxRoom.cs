using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFoxRoom : MonoBehaviour
{
    public GameObject ghostFox;
    public GameObject item;
    public int ghostCount;
    public int itemCount;

    public List<GameObject> sittingCushions;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        while (ghostCount > 0)
        {
            int nowIdx = GetRandomNum();
            if (nowIdx < 0) 
                break;

            Instantiate(ghostFox, sittingCushions[nowIdx].transform.position, Quaternion.identity);
            sittingCushions.RemoveAt(nowIdx);

            ghostCount--;
        }

        while(itemCount > 0)
        {
            int nowIdx = GetRandomNum();
            if (nowIdx < 0)
                break;

            Instantiate(item, sittingCushions[nowIdx].transform.position, Quaternion.identity);
            sittingCushions.RemoveAt(nowIdx);

            itemCount--;
        }

    }

    private int GetRandomNum()
    {
        if(sittingCushions.Count > 0)
        {
            int num = Random.Range(0, sittingCushions.Count);
        }
        return -1;
    }
    
}
