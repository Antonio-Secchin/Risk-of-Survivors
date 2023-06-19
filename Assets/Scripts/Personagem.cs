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
    public Animator animator;
    
    private float directionLR;
    private Vector2 horizontalMovement; // Talvez eu combine isso com directionLR
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

        // Vetor com movimento horizontal do personagem
        horizontalMovement = Vector2.right * speed * directionLR * Time.deltaTime;
        
        transform.Translate(horizontalMovement);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement.x));        

        if (directionLR > 0) {
            gameObject.transform.localScale = new Vector2(1,1);
        }
        else if (directionLR < 0) {
            gameObject.transform.localScale = new Vector2(-1,1);
        }

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
