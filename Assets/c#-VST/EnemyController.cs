using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Jugador que el enemigo va a seguir.
    public Transform player;

    // Distancia horizontal para detectar al jugador.
    public float detectionRadius = 15.0f;

    // Distancia vertical permitida.
    public float verticalRange = 2.0f;

    // Velocidad del enemigo.
    public float speed = 2.0f;

    // Distancia mínima para dejar de avanzar.
    public float distanciaMinima = 0.5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
        {
            DetenerEnemigo();
            return;
        }

        // Distancia horizontal entre enemigo y jugador.
        float distanceX =
            Mathf.Abs(player.position.x - transform.position.x);

        // Distancia vertical entre enemigo y jugador.
        float distanceY =
            Mathf.Abs(player.position.y - transform.position.y);

        // Si Naruto está en otra plataforma,
        // el enemigo no lo persigue.
        if (distanceY > verticalRange)
        {
            DetenerEnemigo();
            return;
        }

        // Persigue al jugador si está dentro del rango.
        if (distanceX <= detectionRadius &&
            distanceX > distanciaMinima)
        {
            float directionX =
                player.position.x - transform.position.x;

            if (directionX > 0)
            {
                movement = Vector2.right;
                spriteRenderer.flipX = false;
            }
            else
            {
                movement = Vector2.left;
                spriteRenderer.flipX = true;
            }

            animator.SetBool("Caminando", true);
        }
        else
        {
            DetenerEnemigo();
        }
    }

    void FixedUpdate()
    {
        // Solo modificamos el movimiento horizontal.
        rb.linearVelocity = new Vector2(
            movement.x * speed,
            rb.linearVelocity.y
        );
    }

    void DetenerEnemigo()
    {
        movement = Vector2.zero;

        if (animator != null)
        {
            animator.SetBool("Caminando", false);
        }
    }
}