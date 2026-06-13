
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        animator.SetFloat("movement",
                          Mathf.Abs(movimiento));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x,
                            fuerzaSalto);

            animator.SetTrigger("jump");
        }
    }
}