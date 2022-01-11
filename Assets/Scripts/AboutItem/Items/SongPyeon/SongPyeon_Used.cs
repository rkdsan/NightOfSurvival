using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPyeon_Used : MonoBehaviour
{
    public Rigidbody rigid;

    private void Awake()
    {
        Vector3 forward = transform.parent.forward;
        forward.y += 0.3f;
        transform.parent = null;
        rigid.velocity = forward * 8f;

        Destroy(gameObject, 5f);
    }

}
