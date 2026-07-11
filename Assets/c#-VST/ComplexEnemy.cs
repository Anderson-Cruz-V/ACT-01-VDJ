using UnityEngine;

public class ComplexEnemy : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRadius = 5f;
    public float attackRadius = 2f;
    public float patrolDistance = 3f;
    public float attackCooldown = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 startPosition;
    private int direction = 1;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            movementStop();
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("Caminando", true);

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (transform.position.x >= startPosition.x + patrolDistance)
        {
            direction = -1;
            Flip();
        }
        else if (transform.position.x <= startPosition.x - patrolDistance)
        {
            direction = 1;
            Flip();
        }
    }

    void FollowPlayer()
    {
        animator.SetBool("Caminando", true);

        float directionToPlayer = player.position.x - transform.position.x;

        if (directionToPlayer > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            spriteRenderer.flipX = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            spriteRenderer.flipX = true;
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("Caminando", false);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Atacar");
            lastAttackTime = Time.time;
        }
    }

    void movementStop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void Flip()
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}