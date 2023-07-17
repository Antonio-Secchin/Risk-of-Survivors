using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public GameObject player;
    PlayerCombat playerCombat;

    public int vidaMax = 100;
    int vidaAtual;
    public float delayMorte = 5f;

    // Start é chamada antes da primeira update de frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMax;
        player = GameObject.FindGameObjectWithTag("Player");
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    public void TakeDamage (int dano, Vector2 knockback) {
        if (vidaAtual <= 0)
            return;
        
        vidaAtual -= dano;

        animator.SetTrigger("Hurt");
        rb.AddForce(knockback, ForceMode2D.Impulse);

        if (vidaAtual <= 0) {
            Die();
        }
    }

    void DaMelhoriaEspecial () {
        // Qual melhoria (por enquanto sem Ataque especial)
        int melhoria = Random.Range(0, 3);
        
        switch (melhoria) {
        case 4:
            // Ataque especial
            break;
        case 3:
            playerCombat.AddVelocidadeAtaque(1, "#3B93F7");
            break;
        case 2:
            playerCombat.AddPulos(2, "Pulos", "#C933FF");
            break;
        case 1:
            playerCombat.AddAlcance(1.2f, "#E4FF33");
            break;
        case 0:
            playerCombat.AddDano(4, "#FF3333");
            break;
        default:
            playerCombat.AddDano(4, "#FF3333");
            break;
        }
    }

    void DaMelhoria () {
        // Qual melhoria
        int melhoria = Random.Range(0, 4);
        
        switch (melhoria) {
        case 4:
            playerCombat.AddVelocidadeAtaque(0.5f, "#0969D7");
            break;
        case 3:
            playerCombat.AddVida();
            break;
        case 2:
            playerCombat.AddPulos(1, "Pulo", "#A500E0");
            break;
        case 1:
            playerCombat.AddAlcance(1.1f, "#C2E000");
            break;
        case 0:
            playerCombat.AddDano(2, "#E00000");
            break;
        default:
            playerCombat.AddDano(2, "#E00000");
            break;
        }
    }

    void Die () {
        animator.SetBool("IsDead", true);

        // Chance de ele dar uma melhoria para o jogador
        if (Random.Range(0, 9) == 1) {
            if (playerCombat.vidaAtual < 100)
                DaMelhoria();
            else
                DaMelhoriaEspecial();
        }

        BoxCollider2D[] colisores = GetComponents<BoxCollider2D>();

        // Para o personagem não ser mais atingido
        foreach (BoxCollider2D c in colisores) {
            // Se eu tirar o que não é trigger, o inimigo atravessa o chão.
            if (c.isTrigger)
                c.enabled = false;
        }

        // Faz o personagem parar de se mover
        MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        Destroy(gameObject, delayMorte);
        this.enabled = false;
    }
}
