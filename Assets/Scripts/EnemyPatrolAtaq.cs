using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        if (animator.GetFloat("Distance") == 1)
            return;
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
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
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

    private void VirarEnemy()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 2f);
        Gizmos.DrawWireSphere(pointB.transform.position, 2f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    public void StopAtack()
    {
        animator.SetFloat("Distance", 0);
    }
}
