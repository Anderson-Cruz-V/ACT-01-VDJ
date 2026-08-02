using UnityEngine;

public class ComplexEnemy : MonoBehaviour
{
    // Referencia al jugador. Se usa para saber dónde está y poder perseguirlo o atacarlo.
    public Transform player;

    // Distancia máxima en X para que el enemigo detecte al jugador.
    public float detectionRange = 5f;

    // Distancia mínima en X para que el enemigo ataque al jugador.
    public float attackRange = 1.5f;

    // Distancia en Y permitida para saber si el jugador está en la misma plataforma.
    public float verticalRange = 1.5f;

    // Velocidad con la que el enemigo camina hacia el jugador.
    public float speed = 2f;

    // Tiempo de espera entre un ataque y otro para que no ataque demasiado rápido.
    public float attackCooldown = 1.5f;

    // Sirve para saber si el enemigo está mirando hacia la derecha.
    private bool facingRight = true;

    // Guarda el Animator del enemigo para activar las animaciones.
    private Animator animator;

    // Guarda el tiempo del último ataque realizado.
    private float lastAttackTime;

    void Start()
    {
        // Se obtiene el componente Animator que tiene el enemigo.
        // Esto permite cambiar entre Idle, Caminar y Ataque.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
        {
            animator.SetBool("Caminando", false);
            return;
        }

        // Calcula la distancia horizontal entre el enemigo y el jugador.
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        // Calcula la distancia vertical entre el enemigo y el jugador.
        float distanceY = Mathf.Abs(player.position.y - transform.position.y);

        // Si el jugador no está en la misma plataforma, el enemigo se queda quieto.
        if (distanceY > verticalRange)
        {
            animator.SetBool("Caminando", false);
            return;
        }

        // Si el jugador está demasiado cerca, el enemigo deja de caminar y ataca.
        if (distanceX <= attackRange)
        {
            animator.SetBool("Caminando", false);
            Attack();
            return;
        }

        // Si el jugador está dentro del rango de detección, el enemigo camina hacia él.
        if (distanceX <= detectionRange)
        {
            FollowPlayer();
        }
        else
        {
            // Si el jugador está lejos, el enemigo se queda en Idle.
            animator.SetBool("Caminando", false);
        }
    }

    void FollowPlayer()
    {
        // Calcula si el jugador está a la derecha o a la izquierda del enemigo.
        float directionX = player.position.x - transform.position.x;

        // Si el jugador está a la derecha y el enemigo mira a la izquierda, se vira.
        if (directionX > 0 && !facingRight)
        {
            Flip();
        }
        // Si el jugador está a la izquierda y el enemigo mira a la derecha, se vira.
        else if (directionX < 0 && facingRight)
        {
            Flip();
        }

        // Activa la animación de caminar.
        animator.SetBool("Caminando", true);

        // Mathf.Sign devuelve 1 si el jugador está a la derecha y -1 si está a la izquierda.
        float movimiento = Mathf.Sign(directionX);

        // Mueve al enemigo solo en X para que no suba ni baje de la plataforma.
        transform.position += Vector3.right * movimiento * speed * Time.deltaTime;
    }

    void Attack()
    {
        // Evita que el enemigo active la animación de ataque muchas veces seguidas.
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Activa la animación de ataque.
            animator.SetTrigger("Atacar");

            // Guarda el momento en que atacó.
            lastAttackTime = Time.time;
        }
    }

    void Flip()
    {
        // Cambia la dirección visual del enemigo.
        facingRight = !facingRight;

        // Invierte la escala en X para que el enemigo mire al otro lado.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}