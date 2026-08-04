using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;

    public float suavizado = 5f;

    // Límites horizontales
    public float limiteIzquierdo = -20f;
    public float limiteDerecho = 70f;

    // Altura fija de la cámara
    public float alturaCamara = 1.5f;

    private Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();

        ActualizarCamara(true);
    }

    void LateUpdate()
    {
        ActualizarCamara(false);
    }

    void ActualizarCamara(bool inmediato)
    {
        if (jugador == null)
            return;

        // Sigue solamente la posición X del jugador
        float posicionX = Mathf.Clamp(
            jugador.position.x,
            limiteIzquierdo,
            limiteDerecho
        );

        Vector3 destino = new Vector3(
            posicionX,
            alturaCamara,
            -10f
        );

        float factor = 1f - Mathf.Exp(
            -suavizado * Time.deltaTime
        );

        transform.position = inmediato
            ? destino
            : Vector3.Lerp(
                transform.position,
                destino,
                factor
            );
    }
}