using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movimiento
    public float velocidad = 5f;
    public float fuerzaSalto = 16f;

    // Daño
    public float tiempoDanio = 0.3f;

    // Vida
    public int vidaMaxima = 3;

    private int vidaActual;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool estaEnSuelo = true;
    private bool recibiendoDanio = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Naruto comienza con la vida máxima
        vidaActual = vidaMaxima;

        Debug.Log("Vida de Naruto: " + vidaActual);
    }

    void Update()
    {
        // Mientras recibe daño no puede moverse
        if (recibiendoDanio)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            animator.SetFloat("movement", 0);

            return;
        }

        // =========================
        // MOVIMIENTO
        // =========================

        float movimiento = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        animator.SetFloat(
            "movement",
            Mathf.Abs(movimiento)
        );

        // =========================
        // GIRAR PERSONAJE
        // =========================

        if (movimiento > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimiento < 0)
        {
            spriteRenderer.flipX = true;
        }

        // =========================
        // SALTO
        // =========================

        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            estaEnSuelo = false;

            animator.SetTrigger("jump");
        }
    }

    // =============================
    // COLISIÓN CON EL SUELO
    // =============================

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
        }
    }

    // =============================
    // CONTACTO CON ENEMIGO
    // =============================

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDanio();
        }
    }

    // =============================
    // RECIBIR DAÑO
    // =============================

    void RecibirDanio()
    {
        // Evita recibir daño varias veces al mismo tiempo
        if (recibiendoDanio)
        {
            return;
        }

        recibiendoDanio = true;

        // Restar una vida
        vidaActual--;

        Debug.Log("Vida de Naruto: " + vidaActual);

        // Detener a Naruto
        rb.linearVelocity = Vector2.zero;

        animator.SetFloat("movement", 0);

        // Animación de daño
        animator.SetTrigger("Danio");

        // Comprobar si murió
        if (vidaActual <= 0)
        {
            Debug.Log("Naruto ha muerto");
        }

        // Después de un tiempo puede volver a moverse
        Invoke(
            nameof(TerminarDanio),
            tiempoDanio
        );
    }

    // =============================
    // TERMINAR DAÑO
    // =============================

    void TerminarDanio()
    {
        recibiendoDanio = false;
    }
}