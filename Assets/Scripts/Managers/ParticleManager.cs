using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    ParticleSystem particle;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    IEnumerator CheckAlive()
    {
        while (true)
        {
            if (!particle.IsAlive())
            {
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(1);
        }
    }
}
