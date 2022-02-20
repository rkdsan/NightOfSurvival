using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassTarget : MonoBehaviour
{
    private void Awake()
    {
        Compass_InHand.target = transform;
    }
}
