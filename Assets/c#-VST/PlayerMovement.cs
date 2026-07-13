using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 12f;
    public float tiempoDanio = 0.3f;

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
    }

    void Update()
    {
        if (recibiendoDanio)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("movement", 0);
            return;
        }

        float movimiento = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        animator.SetFloat("movement", Mathf.Abs(movimiento));

        if (movimiento > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimiento < 0)
        {
            spriteRenderer.flipX = true;
        }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDanio();
        }
    }

    void RecibirDanio()
    {
        if (recibiendoDanio)
        {
            return;
        }

        recibiendoDanio = true;

        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("movement", 0);
        animator.SetTrigger("Danio");

        Invoke(nameof(TerminarDanio), tiempoDanio);
    }

    void TerminarDanio()
    {
        recibiendoDanio = false;
    }
}