using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoCommentSender : MonoBehaviour
{
    public static TutorialManager tutoManager;
    public string comment;
    public bool IsResumable = false;

    private void Reset()
    {
        IsResumable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            tutoManager.ShowTutoWindow(comment);
            if(!IsResumable)
                Destroy(gameObject);
        }
    }


}
