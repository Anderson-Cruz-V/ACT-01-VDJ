using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;
    public Vector3 offset = new Vector3(0, 1.5f, -10f);

    void LateUpdate()
    {
        if (jugador != null)
        {
            Vector3 destino = jugador.position + offset;
            transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
        }
    }
}