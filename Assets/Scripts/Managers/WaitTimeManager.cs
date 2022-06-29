using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeManager
{
    private static WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();

    private static Dictionary<float, WaitForSeconds> waitSeconds 
        = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float second)
    {
        WaitForSeconds wait;
        if(!waitSeconds.TryGetValue(second, out wait))
        {
            waitSeconds.Add(second, wait = new WaitForSeconds(second));
        }

        return wait;
    }

    public static WaitForFixedUpdate WaitForFixedUpdate()
    {
        return waitFixedUpdate;
    }

}
