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

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public bool attackEnable = true;
    public float attackRange = 2;

    [Header("Animator")]
    public Animator animator;

    private Path path;
    private bool isAtacking = false;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
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
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            animator.SetFloat("Speed", 1);
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            if (isGrounded)
            {
                animator.SetFloat("JumpSpeed", 0);
            }
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
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.10f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Attack the enemy
        if (attackEnable && isGrounded)
        {
            if(Vector2.Distance(transform.position, target.transform.position) <= attackRange)
            {
                animator.SetFloat("Distance", 1);
                force = new Vector2(0,0);
                isAtacking = true;
            }
            else if(animator.GetFloat("Distance") == 1)
            {
                animator.SetFloat("Distance", 0);
                force = direction * speed * Time.deltaTime;
                isAtacking = false;
            }
        }
            // Jump
            if (jumpEnabled && isGrounded && !isAtacking)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                animator.SetFloat("JumpSpeed", 1);
                rb.AddForce(Vector2.up * jumpSpeed);
            }
            else if(animator.GetFloat("JumpSpeed")==1)
            {
                animator.SetFloat("JumpSpeed", 0);
                animator.SetFloat("Speed", 1);
            }
        }

        // Movement
        rb.AddForce(force);

        if (!isGrounded) force.y = 0;
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
        if (isGrounded && animator.GetFloat("JumpSpeed") == 1)
        {
            animator.SetFloat("JumpSpeed", 0);

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
}
