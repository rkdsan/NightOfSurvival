using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBottle_InHand : InHandItem
{
    public GameObject sticky_Used;

    public override bool UseItem()
    {
        Vector3 forward = transform.parent.forward;

        //transform.parent는 Used에서 플레이어의 forward를 가져가기 위해 설정
        Instantiate(sticky_Used, transform.position + forward
            , transform.rotation, transform.parent);

        
        return true;
    }

}
