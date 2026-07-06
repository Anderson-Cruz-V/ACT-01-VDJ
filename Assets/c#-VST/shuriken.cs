using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private GameObject efecto; 
    [SerializeField] private float cantidadPuntos = 100f; 
    [SerializeField] private Puntaje puntaje; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (puntaje != null)
            {
                puntaje.SumarPuntos(cantidadPuntos);
            }

            
            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity);
            }

            
            Destroy(gameObject);
        }
    }
}