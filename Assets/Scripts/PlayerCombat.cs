using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D col;
    public Animator animator;
    public PlayerMove playerMove;
    public HealthBar healthBar;
    [SerializeField] GameObject textoUpgrade;

    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;

    [Header("Ataque")]
    public Transform attackPoint;
    public LayerMask layerInimigo;

    public float attackRange = 0.9f;
    public int danoAtaque = 5;
    public int knockback = 50;

    public float attackRate = 2f;
    float nextAttackTime = 0;

    bool pediuAtaque = false;
    int chainAttack = 1;

    [Header("Defesa")]
    public int vidaMax = 100;
    public int vidaAtual;

    public int danoLevado = 10;

    public float defenseRate = 2;
    float nextDefenseTime = 0;

    // Start é chamada antes da primeira update de frame
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        vidaAtual = vidaMax;
        healthBar.SetMaxHealth(vidaMax);
    }

    // Update é chamada uma vez por frame
    void Update()
    {
        // Checa se o player quis atacar quase antes do delay acabar.
        if (Input.GetKeyDown("j") && Time.time >= nextAttackTime - 0.4f/attackRate)
            pediuAtaque = true;

        // Checa se o player já pode atacar de novo
        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown("j") || pediuAtaque) { 
                Ataque();
                nextAttackTime = Time.time + 1f / attackRate;
                pediuAtaque = false;
            }
        }
        
    }

    void ShowFloatingText (string upgrade, string corHexa) {
        var textoTransform = Instantiate(textoUpgrade, transform.position, Quaternion.identity, transform);
        var texto = textoTransform.GetComponent<TextMesh>();

        Debug.Log(corHexa);

        Color cor;
        ColorUtility.TryParseHtmlString(corHexa, out cor);

        Debug.Log(cor);

        texto.text = upgrade;
        texto.color = cor;
    }

    #region upgrades
    // Não preciso de parâmetro, pois nunca vai ser chamada pela DaMelhoriaEspecial
    public void AddVida() { 
        if (vidaAtual >= 80)
            vidaAtual = 100;
        else
            vidaAtual += 20;
            
        healthBar.SetHealth(vidaAtual); 
        ShowFloatingText("Vida", "#00E083");
    }

    public void AddDano (int dano, string cor) { 
        danoAtaque += dano; 
        ShowFloatingText("Dano", cor);
    }

    public void AddAlcance (float mult, string cor) { 
        attackRange *= mult; 
        ShowFloatingText("Alcance", cor);
    }

    public void AddVelocidadeAtaque (float vel, string cor) { 
        attackRate += vel; 
        ShowFloatingText("Velocidade Ataque", cor);
    }

    public void AddPulos (int qtd, string nome, string cor) { 
        playerMove.pulosExtras += qtd; 
        ShowFloatingText(nome, cor);
    }

    #endregion

    private void Ataque () {
        // Ativa a animação de ataque
        attackSoundEffect.Play();
        animator.SetTrigger("Attack");

        // Deteca inimigos dentro do alcance do ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerInimigo);

        // Dá dano nos inimigos
        foreach (Collider2D inimigo in hitEnemies) {
            inimigo.GetComponent<EnemyCombat>().TakeDamage(danoAtaque, knockback * transform.forward);
        }
    }

    public void TakeDamage (int dano, Vector2 knockback) {
        if (vidaAtual <= 0)
            return; // Perdeu o jogo já, para que dar mais dano?
        
        vidaAtual -= dano;

        healthBar.SetHealth(vidaAtual);

        if (vidaAtual <= 0) {
            Die();
        }
        else {
            animator.SetTrigger("Hurt");

            rb.AddForce(knockback, ForceMode2D.Impulse);
            playerMove.sendoJogado = true;
        }
        
    }

    void Die () {
        deathSoundEffect.Play();
        animator.SetBool("IsDead", true);
        playerMove.enabled = false;      
        this.enabled = false;
        Invoke("postDie", 5);
    }

    void postDie ()
    {
        SceneManager.LoadScene(3); // Carrega tela de GameOver
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (Time.time >= nextDefenseTime) {
            if (other.gameObject.tag == "Inimigos") {
                float direcao = Mathf.Sign(transform.position.x - other.transform.position.x);
                TakeDamage(danoLevado, Vector2.right * direcao * 20);
            }
            nextDefenseTime = Time.time + 1/defenseRate;   
        }
    }

    // Desenha no editor o alcance do ataque da espada. Para melhorar nossa visualização como desenvolvedor.
    void OnDrawGizmosSelected () {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
