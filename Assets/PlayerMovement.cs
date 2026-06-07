using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    private Rigidbody2D rb;
    private bool puedeSaltar = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && puedeSaltar)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            puedeSaltar = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        puedeSaltar = true;
    }
}