using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A classe <c> EnemyPatrol()</c> controla todos inimigos que patrulham e nao atacam, ela foi feita para ser facilmente 
/// reutilizada para outros inimigos.
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

        //Configura a velocidade do inimigo para ir na direcao certa
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        //Verifica se chegou no ponto A ou B, vira o inimigo e atualiza o ponto
        if((Vector2.Distance(transform.position, currentPoint.position)) < 1f && currentPoint == pointB.transform)
        {
            VirarEnemy();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointA.transform)
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
        Gizmos.DrawWireSphere(pointA.transform.position, 1f);
        Gizmos.DrawWireSphere(pointB.transform.position, 1f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
