using UnityEngine;
using UnityEngine.SceneManagement; // Para cargar escenas
using UnityEngine.UI; // Para trabajar con botones

public class WinLevel2 : MonoBehaviour
{
    public Button ButtonSalir;          // Referencia al botón de salir
    public GameObject CanvasWin;        // Referencia al Canvas del Win
    public GameObject CanvasVida;       // Referencia al Canvas de Vida
    public GameObject CanvasScore;      // Referencia al Canvas de Score
    public GameObject CanvasBotonPausa; // Referencia al Canvas de Botones de Pausa

    void Start()
    {
        // Asegúrate de que el CanvasWin está activo
        if (CanvasWin != null)
        {
            CanvasWin.SetActive(true); // Activar el canvas de Win si está desactivado
        }
        else
        {
            Debug.LogError("CanvasWin no está asignado en el Inspector.");
        }

        // Asegúrate de que los canvas CanvasVida, CanvasScore y CanvasBotonPausa están correctamente asignados
        if (CanvasVida != null && CanvasScore != null && CanvasBotonPausa != null)
        {
            // Desactivar los otros Canvas
            CanvasVida.SetActive(false);
            CanvasScore.SetActive(false);
            CanvasBotonPausa.SetActive(false);
        }
        else
        {
            Debug.LogError("Uno o más Canvas no están asignados en el Inspector.");
        }

        // Asegúrate de que el botón está correctamente referenciado
        if (ButtonSalir != null)
        {
            ButtonSalir.onClick.AddListener(GoToMenuNiveles); // Asignar función al botón Salir
        }
        else
        {
            Debug.LogError("ButtonSalir no está asignado en el Inspector.");
        }
    }

    // Método para ir al menú de niveles
    private void GoToMenuNiveles()
    {
        Debug.Log("Botón Salir presionado"); // Mensaje para confirmar interacción

        // Asegurarse de que el tiempo está en escala normal
        Time.timeScale = 1f;

        // Cargar la escena del menú de niveles
        SceneManager.LoadScene("MenuNiveles"); // Asegúrate de que el nombre de la escena sea correcto
    }
}
