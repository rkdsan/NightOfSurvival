using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman_Used : MonoBehaviour
{
    public GameObject explosionParticle;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            other.GetComponent<Ghost>().Stuned(2);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            EffectSoundManager.instance.effectSound_boom.Play();
            Destroy(gameObject);
        }
    }

}
