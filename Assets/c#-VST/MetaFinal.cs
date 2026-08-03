using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaFinal : MonoBehaviour
{
    public GameObject panelVictoria;

    private bool nivelTerminado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !nivelTerminado)
        {
            nivelTerminado = true;

            panelVictoria.SetActive(true);

            Time.timeScale = 0f;

            Debug.Log("¡NIVEL COMPLETADO!");
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void SalirDelJuego()
    {
        Time.timeScale = 1f;

        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}