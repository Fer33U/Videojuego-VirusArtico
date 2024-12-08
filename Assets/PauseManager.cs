using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Canvas canvasBotonPausa; // Asigna el Canvas del botón de pausa
    public Canvas canvasPausa;     // Asigna el Canvas del menú de pausa
    public Button botonPausa;      // Asigna el botón de pausa

    private bool isPaused = false; // Indica si el juego está en pausa

    void Start()
    {
        // Asegúrate de que los Canvas estén en el estado inicial
        canvasPausa.gameObject.SetActive(false);
        canvasBotonPausa.gameObject.SetActive(true);

        // Asigna el evento del botón
        botonPausa.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        // Detecta si se presiona la tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        // Alterna entre activar/desactivar los Canvas
        canvasPausa.gameObject.SetActive(isPaused);
        canvasBotonPausa.gameObject.SetActive(!isPaused);

        // Opcional: Detener el tiempo cuando está en pausa
        Time.timeScale = isPaused ? 0 : 1;
    }
}
