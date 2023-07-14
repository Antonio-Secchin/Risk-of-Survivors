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
        // Qual melhoria
        int melhoria = Random.Range(0, 4);
        // DEBUG
        melhoria = 3;
        Debug.Log("Especial: " + melhoria.ToString());
        
        switch (melhoria) {
        case 4:
            playerCombat.AddVelocidadeAtaque();
            break;
        case 3:
            Debug.Log("Add nada");
            break;
        case 2:
            playerCombat.AddPulos();
            break;
        case 1:
            playerCombat.AddAlcance();
            break;
        case 0:
            playerCombat.AddDano();
            break;
        default:
            playerCombat.AddDano();
            break;
        }
    }

    void DaMelhoria () {
        // Qual melhoria
        int melhoria = Random.Range(0, 4);
        // DEBUG
        melhoria = 3;
        Debug.Log(melhoria.ToString());
        
        switch (melhoria) {
        case 4:
            playerCombat.AddVelocidadeAtaque();
            break;
        case 3:
            playerCombat.AddVida();
            break;
        case 2:
            playerCombat.AddPulos();
            break;
        case 1:
            playerCombat.AddAlcance();
            break;
        case 0:
            playerCombat.AddDano();
            break;
        default:
            playerCombat.AddDano();
            break;
        }
    }

    void Die () {
        animator.SetBool("IsDead", true);

        // Chance de ele dar uma melhoria para o jogador
        if (Random.Range(1, 1) == 1) {
            Debug.Log("Matou! Vida antes upgrade: " + playerCombat.vidaAtual);
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
