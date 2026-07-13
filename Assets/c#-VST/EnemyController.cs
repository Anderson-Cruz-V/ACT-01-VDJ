using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 1f;

    private Vector3 startingPosition;
    public Vector3 rightPosition;
    public Vector3 leftPosition;
    public Vector3 currentPosition;
    public float currentDistance;

    private bool movingRight = true;

    void Start()
    {
        currentPosition = transform.position;
        startingPosition = transform.position;

        // Se calcula hasta dónde puede caminar el enemigo hacia la derecha.
        rightPosition = startingPosition + Vector3.right * distance;

        // Se calcula hasta dónde puede caminar el enemigo hacia la izquierda.
        leftPosition = startingPosition + Vector3.left * distance;
    }

    void Update()
    {
        currentDistance = Vector2.Distance(transform.position, startingPosition);

        // Si el enemigo está caminando hacia la derecha.
        if (movingRight)
        {
            // El enemigo camina hacia la derecha.
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Cuando llega a la distancia indicada, se vira.
            if (Vector2.Distance(transform.position, startingPosition) >= distance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            // El enemigo camina hacia la izquierda.
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Cuando llega a la distancia indicada, se vira otra vez.
            if (Vector2.Distance(transform.position, startingPosition) >= distance)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        // Cambia la dirección visual del enemigo.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}