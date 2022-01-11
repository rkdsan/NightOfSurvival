using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPyeon : OnGroundItem
{
    public Rigidbody rigid;
    public SphereCollider spCollider;


    public bool UseItem()
    {
        Vector3 forward = transform.parent.forward;
        forward.y += 0.3f;
        GameObject copy = Instantiate(gameObject, transform.position + forward, transform.rotation);

        SongPyeon copySP = copy.GetComponent<SongPyeon>();

        copySP.rigid.isKinematic = false;
        copySP.rigid.velocity = forward * 8f;
        copySP.spCollider.enabled = true;

        copySP.DestoryThis();

        EffectSoundManager.instance.effectSound_pickUp.Play();
        return true;
    }

    public void DestoryThis()
    {
        StartCoroutine(DestoryTimer());
    }

    IEnumerator DestoryTimer()
    {
        yield return WaitTimeManager.WaitForSeconds(5);
        Destroy(gameObject);
    }

}
