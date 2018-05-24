using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviourExt
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ObjectController.GetComponent<ObjectController>().IsChild(collision.transform) == false)
        {
            transform.parent.gameObject.SendMessage("OnCollisionEnter2D", collision);
        }
    }
}