using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviourExt
{
    const float airResistanceForce = 0.05f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Item")
        {
            if (collision.collider.tag == "Player")
            {
                ItemPickup(collision.collider.transform);
                foreach (Rigidbody2D rb in ObjectController.Child)
                {
                    rb.tag = "Parachute";
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.layer == Layer.Parachute)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, airResistanceForce), ForceMode2D.Impulse);
        }
    }

    private void ItemPickup(Transform collider)
    {
        Transform target = collider.parent;
        SetParachuteAttributes(target);
        CreateJoint(target);
        //SetParachutePositions(target);
    }

    private void SetParachuteAttributes(Transform target)
    {
        transform.parent = target;
        foreach (Rigidbody2D rb in ObjectController.Child)
        {
            rb.gameObject.layer = Layer.Parachute;
            rb.transform.eulerAngles = Vector3.zero;
            if (rb.GetComponent<Collider2D>() != null)
            {
                rb.GetComponent<Collider2D>().enabled = false;
            }

        }
        transform.eulerAngles = new Vector3(0, 0, 180);
        ObjectController.NullifyVelocities();
        if (transform.GetComponent<Collider2D>() != null)
        {
            transform.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void CreateJoint(Transform target)
    {
        DistanceJoint2D joint = gameObject.AddComponent<DistanceJoint2D>();
        joint.connectedBody = target.GetComponent<Rigidbody2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
        joint.autoConfigureDistance = false;
        joint.distance = 2;
    }

    private void SetParachutePositions(Transform target)
    {
        InventoryController inventory = target.GetComponent<InventoryController>();
        inventory.Parachute.Add(transform);
        const float distance = 2;
        const float separationAngle = 10;
        float totalAngle = (inventory.Parachute.Count - 1) * separationAngle;
        float startingAngle = -(totalAngle / 2);
        for (int i = 0; i < inventory.Parachute.Count; i++)
        {
            float currentAngle = startingAngle + (separationAngle * i);
            Vector3 position = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), Mathf.Cos(Mathf.Deg2Rad * currentAngle), 0);
            position *= distance;
            position += target.position;
            inventory.Parachute[i].position = position;
            inventory.Parachute[i].GetComponent<DistanceJoint2D>().anchor = Vector3.zero;
            inventory.Parachute[i].GetComponent<DistanceJoint2D>().connectedAnchor = target.position;
        }
    }
}
