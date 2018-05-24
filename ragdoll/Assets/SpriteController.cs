using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviourExt
{
    public Color globalColor = Color.black;

    private void Start()
    {
        SpriteRenderer sprite = new SpriteRenderer();
        foreach (Rigidbody2D rb in ObjectController.Child)
        {
            sprite = rb.GetComponent<SpriteRenderer>();
            if (sprite != null)
                sprite.color = globalColor;
        }
    }
}
