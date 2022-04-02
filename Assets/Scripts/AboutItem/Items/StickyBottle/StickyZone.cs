using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyZone : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Vector3 setPos = transform.position;
        //땅에 붙는게 0.1 나중에 수정요망
        setPos.y = 0.1f;
        transform.position = setPos;

        Destroy(gameObject, 8);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameData.GHOST_TAG))
        {
            Debug.Log("고스트 엔터");
            other.GetComponent<Ghost>().SetSlow(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameData.GHOST_TAG))
        {
            Debug.Log("고스트 아웃");
            other.GetComponent<Ghost>().SetSlow(false);
        }
    }

}
