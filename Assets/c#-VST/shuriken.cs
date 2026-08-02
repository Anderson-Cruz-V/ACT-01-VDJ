using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private GameObject efecto; 
    [SerializeField] private float cantidadPuntos = 100f; 
    [SerializeField] private Puntaje puntaje; 

    private void Awake()
    {
        if (puntaje == null)
        {
            puntaje = FindAnyObjectByType<Puntaje>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (puntaje != null) puntaje.SumarPuntos(cantidadPuntos);
            GameAudioManager.Play(GameSound.CollectPoints, 0.05f);
            GameAudioVfx.Burst(transform.position, new Color(1f, .78f, .08f, .95f), 16, .15f);

            
            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity);
            }

            
            Destroy(gameObject);
        }
    }
}
