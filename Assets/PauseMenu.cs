using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvasBotonPausa; // Asigna el Canvas del botón de pausa
    public Canvas canvasPausa;     // Asigna el Canvas del menú de pausa
    public Button botonReanudar;   // Asigna el botón de reanudar
    public Button botonSalir;      // Asigna el botón de salir

    void Start()
    {
        // Asegúrate de que los botones estén configurados correctamente
        botonReanudar.onClick.AddListener(ReanudarJuego);
        botonSalir.onClick.AddListener(SalirAlMenu);
    }

    public void ReanudarJuego()
    {
        // Reanudar el juego
        Time.timeScale = 1;
        canvasPausa.gameObject.SetActive(false);
        canvasBotonPausa.gameObject.SetActive(true);
    }

    public void SalirAlMenu()
    {
        // Reanuda el tiempo (por si estaba en pausa) y carga la escena del menú
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuNiveles");
    }
}
