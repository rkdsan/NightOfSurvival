using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public string objectName;
    public string explainComment;

    public abstract void Interact();

    

}
