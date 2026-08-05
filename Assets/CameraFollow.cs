using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;

    // Velocidad con la que la cámara sigue al jugador
    public float suavizado = 5f;

    // Límites horizontales del escenario
    public float limiteIzquierdo = -15f;
    public float limiteDerecho = 90f;

    // La cámara NO seguirá el salto de Naruto
    public float alturaCamara = 1.5f;

    void Start()
    {
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

        // Seguir solamente a Naruto horizontalmente
        float posicionX = Mathf.Clamp(
            jugador.position.x,
            limiteIzquierdo,
            limiteDerecho
        );

        // Y siempre permanece fija
        Vector3 destino = new Vector3(
            posicionX,
            alturaCamara,
            -10f
        );

        if (inmediato)
        {
            transform.position = destino;
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                destino,
                suavizado * Time.deltaTime
            );
        }
    }
}