using UnityEngine;

public class ComplexEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float verticalRange = 1.2f;
    public float speed = 2f;

    private bool facingRight = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceY = Mathf.Abs(player.position.y - transform.position.y);

        // Si el jugador no está en la misma plataforma, el enemigo se queda quieto.
        if (distanceY > verticalRange)
        {
            animator.SetBool("Caminando", false);
            return;
        }

        // Si el jugador está muy cerca, el enemigo ataca.
        if (distanceX <= attackRange)
        {
            animator.SetBool("Caminando", false);
            animator.SetTrigger("Atacar");
            return;
        }

        // Si el jugador está cerca, el enemigo camina hacia él.
        if (distanceX <= detectionRange)
        {
            float directionX = player.position.x - transform.position.x;

            if (directionX > 0 && !facingRight)
            {
                Flip();
            }
            else if (directionX < 0 && facingRight)
            {
                Flip();
            }

            animator.SetBool("Caminando", true);

            float movimiento = Mathf.Sign(directionX);
            transform.Translate(Vector2.right * movimiento * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Caminando", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}