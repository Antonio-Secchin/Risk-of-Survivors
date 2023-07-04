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

    public float moveSpeed;
    public float jumpSpeed;
    public float aceleracao;
    public float desaceleracao;
    [SerializeField] float velPower; // [SerializeField] == apareça no inspector, mesmo se for private
    public float pulosExtras;

    #endregion

    bool pediuPular = false;
    float moveInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        #region regulação de movimento

        moveSpeed = 20.0f;
        jumpSpeed = 23.0f;
        aceleracao = 1.0f;
        desaceleracao = 1.0f;
        velPower = 2;
        pulosExtras = 1;

        #endregion

        rb= GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update é chamado continuamente
    void Update () {
        // pediuPular é false até o jogador apertar para pular.
        if (pediuPular == false) 
            pediuPular = Input.GetButtonDown("Jump");

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

        animator.SetBool("Jump", !IsGrounded());

    }

    private bool IsGrounded() {
        // Isso cria uma caixa do tamanho da hitbox, mas 0.1f abaixo dela. Checa se isso colide com algo que tenha o layer chaoPulavel, se sim retorna true.
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, chaoPulavel);
    }

    private void Pulo () {
        // Não dá para pular, nesse caso.
        if (!IsGrounded() && pulosExtras <= 0) {
            return;
        }

        if (!IsGrounded()) {
            //animator.SetBool("JetPack", true);
            pulosExtras--;
        }
        else{
            animator.SetBool("Jump", true);
            pulosExtras = 1;
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
