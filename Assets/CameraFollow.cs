using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;
    public Vector3 offset = new Vector3(0, 0f, -10f);
    public float tamanoOrtografico = 6.5f;

    private Camera camara;
    private SpriteRenderer fondo;

    void Start()
    {
        camara = GetComponent<Camera>();
        if (camara != null)
        {
            camara.orthographic = true;
            camara.orthographicSize = tamanoOrtografico;
        }

        GameObject fondoObject = GameObject.Find("Imagen_0");
        if (fondoObject != null)
        {
            fondo = fondoObject.GetComponent<SpriteRenderer>();
        }

        ActualizarCamara(true);
    }

    void LateUpdate()
    {
        ActualizarCamara(false);
    }

    void ActualizarCamara(bool inmediato)
    {
        if (jugador == null || camara == null)
        {
            return;
        }

        Vector3 destino = jugador.position + offset;

        if (fondo != null)
        {
            Bounds limites = fondo.bounds;
            float mitadAlto = camara.orthographicSize;
            float mitadAncho = mitadAlto * camara.aspect;

            float minimoX = limites.min.x + mitadAncho;
            float maximoX = limites.max.x - mitadAncho;
            destino.x = minimoX <= maximoX ? Mathf.Clamp(destino.x, minimoX, maximoX) : limites.center.x;

            float minimoY = limites.min.y + mitadAlto;
            float maximoY = limites.max.y - mitadAlto;
            destino.y = minimoY <= maximoY ? Mathf.Clamp(destino.y, minimoY, maximoY) : limites.center.y;
        }

        destino.z = -10f;
        float factor = 1f - Mathf.Exp(-suavizado * Time.deltaTime);
        transform.position = inmediato ? destino : Vector3.Lerp(transform.position, destino, factor);
    }
}
