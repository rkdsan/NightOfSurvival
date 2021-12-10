using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticle : MonoBehaviour
{
    public ParticleSystem ps;
    void Start()
    {
        Debug.Log("수정 전 burstCount: " + ps.emission.burstCount);

        ParticleSystem.EmissionModule em = ps.emission;
        em.burstCount = 5;

        Debug.Log("수정 후 burstCount: " + ps.emission.burstCount);


    }

}
