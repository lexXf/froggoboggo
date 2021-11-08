using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poof : MonoBehaviour
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
            gameObject.transform.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 0.2f);
        }
    }

    void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
