using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [HideInInspector]
    public List<Rigidbody2D> Child;

    public Vector2 Orientation
    {
        get
        {
            Vector2 orientation = new Vector2(
                Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180),
                Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180));
            return orientation;
        }
        set
        {
            float newOrientation = Vector2.SignedAngle(new Vector2(0, 1), (value - transform.GetComponent<Rigidbody2D>().position));
            if (newOrientation < 0)
            {
                newOrientation = 360 + newOrientation;
            }
            transform.eulerAngles = new Vector3(0, 0, newOrientation);
        }
    }

    public bool FreezeRotation
    {
        get { return transform.GetComponent<Rigidbody2D>().freezeRotation; }
        set
        {
            for (int i = 0; i < Child.Count; i++)
            {
                NullifyAngularVelocity();
                Child[i].freezeRotation = value;
            }
        }
    }

    void Start()
    {
        Child = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
    }

    public bool IsChild(Transform child)
    {
        return Child.Contains(child.GetComponent<Rigidbody2D>());
    }

    public static Vector2 GetOrientation(Transform transform)
    {
            Vector2 orientation = new Vector2(
                Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180),
                Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180));
            return orientation;
    }

    public float Angle(Vector2 target)
    {
        return Vector2.Angle(target - new Vector2(transform.position.x, transform.position.y), Orientation);
    }

    public static float Angle(Transform origin, Vector2 target)
    {
        return Vector2.Angle(target - new Vector2(origin.position.x, origin.position.y), GetOrientation(origin));
    }


    public void NullifyAngularVelocity()
    {
        for (int i = 0; i < Child.Count; i++)
        {
            Child[i].angularVelocity = 0;
        }
    }

    public void NullifyVelocity()
    {
        for (int i = 0; i < Child.Count; i++)
        {
            Child[i].velocity = Vector2.zero;
        }
    }
    public void NullifyVelocities()
    {
        for (int i = 0; i < Child.Count; i++)
        {
            Child[i].angularVelocity = 0;
            Child[i].velocity = Vector2.zero;
        }
    }

}
