using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBottle_Used : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameData.GHOST_TAG))
        {
            other.GetComponent<Ghost>().GetSlow();
        }
    }


}
