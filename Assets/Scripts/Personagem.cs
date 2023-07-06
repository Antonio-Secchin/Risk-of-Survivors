using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Personagem : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D col;
    public Animator animator;

    [SerializeField] LayerMask chaoPulavel;

    #region Variáveis correr

    public float moveSpeed = 15.0f;
    public float jumpSpeed = 15.0f;
    public float aceleracao = 1.0f;
    public float desaceleracao = 1.0f;
    [SerializeField] float velPower = 2; // [SerializeField] == apareça no inspector, mesmo se for private
    public int pulosExtras = 1;
    public int pulosAtuais = 1;

    #endregion

    bool pediuAtacar = false;
    bool pediuPular = false;
    float moveInput = 0;

    // Start é chamada antes da primeira update de frame
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update é chamado continuamente
    void Update () {
        // Desse jeito só pode ser pedido um pulo até FixedUpdate rodar.
        if (pediuPular == false) 
            pediuPular = Input.GetButtonDown("Jump");
        
        // Desse jeito só pode ser pedido um ataque até FixedUpdate rodar.
        if (pediuAtacar == false) 
            pediuAtacar = Input.GetKeyDown("j");

        // Seta a yVelocity no animador
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    // FixedUpdate é chamado toda vez que a engine de física rodar. Como não é todo frame, melhor receber os inputs muito curtos no Update.
    void FixedUpdate()
    {

        #region correr
        moveInput = Input.GetAxis("Horizontal");

        // Velocidade (e direção) que querermos alcançar
        float targetSpeed = moveInput * moveSpeed;
        // Pega a diferença entre a velocidade desejável e atual
        float speedDiff = targetSpeed - rb.velocity.x;
        // Aceleramos quando apertamos o botão. Desaceleramos quando não apertamos nada. Podemos querer que um aconteça mais rápido do que o outro, então separamos as situações.
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? aceleracao : desaceleracao;
        // Multiplica a diferença de velocidade com a aceleração. Depois eleva isso a um número que quisermos. E finalmente, multiplicamos para voltar com o sinal.
        float movimento = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
        
        rb.AddForce(movimento * Vector2.right);

        #endregion
        
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));  

        ViraPersonagem();
        
        if (pediuPular) {
            pediuPular = false;    
            Pulo();
        }

        if (pediuAtacar) {
            Ataque();
            pediuAtacar = false;    
        }

        if (IsGrounded()) {
            if (pulosAtuais <= 0) {
                pulosAtuais = pulosExtras;
            }
            animator.SetBool("Jump", false);
        }
        else
            animator.SetBool("Jump", true);

    }

    private bool IsGrounded() {
        // Isso cria uma caixa do tamanho da hitbox, mas 0.1f abaixo dela. Checa se isso colide com algo que tenha o layer chaoPulavel, se sim retorna true.
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, chaoPulavel);
    }

    private void Ataque () {
        animator.SetTrigger("Attack");
    }

    private void Pulo () {
        // Não dá para pular, nesse caso.
        if (!IsGrounded() && pulosAtuais <= 0) {
            return;
        }

        if (!IsGrounded()) {
            //animator.SetBool("JetPack", true);
            pulosAtuais--;
        }
        else{
            animator.SetBool("Jump", true);
            pulosAtuais = 1;
        }

        rb.AddForce(transform.up * jumpSpeed + new Vector3(0, -rb.velocity.y, 0), ForceMode2D.Impulse);
    }

    private void ViraPersonagem () {
        if (moveInput > 0) {
            gameObject.transform.localScale = new Vector2(1,1);
        }
        else if (moveInput < 0) {
            gameObject.transform.localScale = new Vector2(-1,1);
        }
    }

}
