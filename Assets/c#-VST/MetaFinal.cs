using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MetaFinal : MonoBehaviour
{
    // Panel que aparece al terminar el nivel
    public GameObject panelVictoria;

    // Texto del puntaje dentro del PanelVictoria
    public TMP_Text textoPuntaje;

    // Referencia al sistema de puntaje
    public Puntaje puntaje;

    // Evita que la meta se active varias veces
    private bool nivelTerminado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mostrar en consola qué objeto tocó la meta
        Debug.Log(
            "META TOCADA POR: " + collision.name +
            " | Posición meta: " + transform.position
        );

        // Comprobar que quien tocó la meta sea el jugador
        if (collision.CompareTag("Player") && !nivelTerminado)
        {
            nivelTerminado = true;

            // Mostrar el panel de victoria
            panelVictoria.SetActive(true);

            // Mostrar el puntaje obtenido
            if (puntaje != null && textoPuntaje != null)
            {
                textoPuntaje.text = $"PUNTAJE: {puntaje.Puntos:0000}";
            }

            // Pausar el juego
            Time.timeScale = 0f;

            Debug.Log(
                "¡NIVEL COMPLETADO! Jugador llegó a la meta en: "
                + transform.position
            );
        }
    }

    public void ReiniciarNivel()
    {
        // Volver a activar el tiempo
        Time.timeScale = 1f;

        // Reiniciar la escena actual
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void SalirDelJuego()
    {
        // Restaurar el tiempo
        Time.timeScale = 1f;

        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
}