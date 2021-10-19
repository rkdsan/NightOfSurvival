using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTestItem : Item
{
    public override void UseItem()
    {
        Vector3 forward = transform.parent.forward;
        forward.y += 0.3f;
        GameObject copy = Instantiate(gameObject, transform.position + forward, transform.rotation);

        Rigidbody rigid = copy.AddComponent<Rigidbody>();
        rigid.velocity = forward * 8f;
    }

}
