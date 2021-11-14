using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNamePrinter : MonoBehaviour
{
    public Text itemText;

    private int checker;

    public ItemNamePrinter()
    {
        checker = int.MinValue;
    }

    public void OnText(string name)
    {
        checker++;
        itemText.text = name;
        itemText.enabled = true;

        StartCoroutine(OffTimer(checker));
    }

    IEnumerator OffTimer(int ck)
    {
        yield return new WaitForSeconds(1.5f);

        if (ck == checker)//템이 안바꼈으면
        { 
            itemText.enabled = false;
        }
    }

}
