using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBottle_Used : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject sticky;

    private void Awake()
    {
        Vector3 forward = transform.parent.forward;
        forward.y += 0.3f;
        transform.parent = null;
        rigid.velocity = forward * 8f;
        StartCoroutine(CheckGround());
    }

    private IEnumerator CheckGround()
    {
        while (rigid.velocity.magnitude > 2)
        {
            yield return WaitTimeManager.WaitForFixedUpdate();
        }

        //º´ÀÌ ±úÁü
        Instantiate(sticky, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
