using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform camara;
    private Vector3 posicionAnterior;

    [Range(0f, 1f)]
    public float efectoParallax = 0.5f;

    void Start()
    {
        camara = Camera.main.transform;
        posicionAnterior = camara.position;
    }

    void LateUpdate()
    {
        Vector3 movimiento = camara.position - posicionAnterior;

        transform.position += new Vector3(
            movimiento.x * efectoParallax,
            movimiento.y * efectoParallax,
            0
        );

        posicionAnterior = camara.position;
    }
}