using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// A classe <c> EnemyPatrolAtaq()</c> controla todos inimigos que patrulham e atacam, ela foi feita para ser facilmente 
/// reutilizada para outros inimigos.
/// </summary>
public class EnemyPatrolAtaq : MonoBehaviour
{
    [Header("Variables")]
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 20;
    public float attackRange;
    public GameObject player;

    [Header("Animator")]
    public Animator animator;

    private Rigidbody2D rb;
    private Transform currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Verifica se o inimigo esta fazendo a animacao de ataque
        if (animator.GetFloat("Distance") == 1)
            return;

        //Verifica se o jogador esta dentro do alcance de ataque e prepara o inmigo para atacar
        //Virando ele para a direcao certa, parando a velocidade e comecando a animacao
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if(transform.position.x < player.transform.position.x && currentPoint == pointA.transform) 
            {
                VirarEnemy();
                currentPoint = pointB.transform;
            }
            else if(transform.position.x > player.transform.position.x && currentPoint == pointB.transform)
            {
                VirarEnemy();
                currentPoint = pointA.transform;
            }
            rb.velocity = new Vector2(0, 0);
            animator.SetFloat("Distance", 1);
        }

        Vector2 point = currentPoint.position - transform.position;
        //Configura a velocidade do inimigo para ir na direcao certa
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        //Verifica se chegou no ponto A ou B, vira o inimigo e atualiza o ponto
        if ((Vector2.Distance(transform.position, currentPoint.position)) < 2f && currentPoint == pointB.transform)
        {
            VirarEnemy();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 2f && currentPoint == pointA.transform)
        {
            VirarEnemy();
            currentPoint = pointB.transform;
        }
    }

    /// <summary>
    /// A funcao <c> VirarEnemy()</c> é utilizada para virar a sprite do inimigo para a direcao que ele esta andando.
    /// </summary>
    private void VirarEnemy()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /// <summary>
    /// A funcao <c> OnDrawGizmos()</c> desenha os pontos A e B no cenario.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 2f);
        Gizmos.DrawWireSphere(pointB.transform.position, 2f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    /// <summary>
    /// A funcao <c> StopAtack()</c> eh chamada quando a animacao de ataque acaba.
    /// </summary>
    public void StopAtack()
    {
        animator.SetFloat("Distance", 0);
    }
}
