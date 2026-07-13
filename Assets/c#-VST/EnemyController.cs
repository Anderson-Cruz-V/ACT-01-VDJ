using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Jugador que el enemigo va a seguir.
    public Transform player;

    // Distancia horizontal para detectar al jugador.
    // Si está muy bajo, el enemigo solo corre cuando el jugador está pegado.
    public float detectionRadius = 15.0f;

    // Distancia vertical permitida.
    // Sirve para saber si el jugador está en el mismo suelo.
    public float verticalRange = 2.0f;

    // Velocidad del enemigo.
    public float speed = 2.0f;

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
            movement = Vector2.zero;
            animator.SetBool("Caminando", false);
            return;
        }

        // Distancia izquierda/derecha entre enemigo y jugador.
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        // Distancia arriba/abajo entre enemigo y jugador.
        float distanceY = Mathf.Abs(player.position.y - transform.position.y);

        // Si el jugador está en otra plataforma, el enemigo se queda quieto.
        if (distanceY > verticalRange)
        {
            movement = Vector2.zero;
            animator.SetBool("Caminando", false);
            return;
        }

        // Si el jugador está en el mismo suelo y dentro del rango, lo persigue.
        if (distanceX <= detectionRadius)
        {
            float directionX = player.position.x - transform.position.x;

            if (directionX > 0)
            {
                // Jugador a la derecha.
                movement = new Vector2(1, 0);
                spriteRenderer.flipX = false;
            }
            else if (directionX < 0)
            {
                // Jugador a la izquierda.
                movement = new Vector2(-1, 0);
                spriteRenderer.flipX = true;
            }

            animator.SetBool("Caminando", true);
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("Caminando", false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}