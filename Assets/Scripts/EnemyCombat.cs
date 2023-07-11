using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public int vidaMax = 100;
    int vidaAtual;
    public float delayMorte = 5f;

    // Start é chamada antes da primeira update de frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMax;
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

    void Die () {
        animator.SetBool("IsDead", true);
        
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
