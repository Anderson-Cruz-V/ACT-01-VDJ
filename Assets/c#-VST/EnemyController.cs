using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rd;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            movement = new Vector2(direction.x, 0);

            animator.SetBool("Caminando", true);

            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("Caminando", false);
        }
    }

    void FixedUpdate()
    {
        rd.MovePosition(rd.position + movement * speed * Time.fixedDeltaTime);
    }
}