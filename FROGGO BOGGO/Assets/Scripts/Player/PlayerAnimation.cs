using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    private SpriteRenderer spriteR;
    public Sprite[] spriteArray;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            spriteR.sprite = spriteArray[0];
        }
        else
        {
            spriteR.sprite = spriteArray[1];
        }
    }
}
