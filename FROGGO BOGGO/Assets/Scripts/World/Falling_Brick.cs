using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Brick : MonoBehaviour
{

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name.Equals ("Froggo Boggo") || col.gameObject.name.Equals("Grappling Gun"))
        {
            Invoke("DropPlatform", 0.2f);
            Destroy(gameObject, 2f);
        }
    }

    void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
