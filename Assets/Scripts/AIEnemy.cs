using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float jumpSpeed = 1000f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    //public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public Vector3 boxSize;
    public float maxDistGround;
    public LayerMask Ground;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public bool attackEnable = true;
    public float attackRange = 2;

    [Header("Animator")]
    public Animator animator;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0.5f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if(animator.GetFloat("Distance") == 1)
        {
            return;
        }
        if (TargetInDistance() && followEnabled)
        {
            animator.SetFloat("Speed", 1);
            PathFollow();
        }
        else
        {
            animator.SetFloat("Speed", 0);
            if (IsGrounded())
            {
                animator.SetFloat("JumpSpeed", 0);
            }

        }
    }

        private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }


        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //is jumping
        if (!IsGrounded())
        {
            force.y = 0;
            //Esse addforce eh duplicado, mas gostei mais da movimenta��o dele assim, ficou mais fluida durante o pulo
            rb.AddForce(force);
        }
        else if (animator.GetFloat("JumpSpeed") == 1)
        {
            animator.SetFloat("JumpSpeed", 0);
        }


        //Attack the enemy
        if (attackEnable && IsGrounded())
        {
            if(Vector2.Distance(transform.position, target.transform.position) <= attackRange)
            {
                animator.SetFloat("Distance", 1);
            }
            else if(animator.GetFloat("Distance") == 1)
            {
                animator.SetFloat("Distance", 0);
            }
        }
        // Jump
        if (jumpEnabled && IsGrounded())
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                animator.SetFloat("JumpSpeed", 1);
                rb.AddForce(Vector2.up * jumpSpeed);
            }
        }

        // Movement
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private bool IsGrounded()
    {
        Vector2 aux = transform.position;
        aux.y = aux.y - 0.5f;
        if (Physics2D.BoxCast(aux, boxSize, 0, -transform.up, maxDistGround, Ground))
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 aux = transform.position;
        aux.y = aux.y - 0.5f;
        Gizmos.DrawCube(aux - transform.up * maxDistGround, boxSize);
    }

    public void StopAtack()
    {
        animator.SetFloat("Distance", 0);
    }
}
