using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoCommentSender : MonoBehaviour
{
    public static TutorialManager tutoManager;
    public string comment;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutoManager.ShowTutoWindow(comment);
            Destroy(gameObject);
        }
    }


}
