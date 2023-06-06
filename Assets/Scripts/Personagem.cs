using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

public class Personagem : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 30.0f;
    private float directionLR;
    private float jumpInput;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionLR = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * speed * directionLR * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
