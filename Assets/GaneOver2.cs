using UnityEngine;
using UnityEngine.SceneManagement; // Para cargar escenas
using UnityEngine.UI; // Para trabajar con botones

public class GameOverLv2 : MonoBehaviour
{
    public Button ButtonReintentar; // Referencia al botón de reiniciar
    public Button ButtonSalir;      // Referencia al botón de salir
    public GameObject CanvasGameOver; // Referencia opcional al Canvas del GameOver

    void Start()
    {
        // Asegúrate de que el CanvasGameOver está activo
        if (CanvasGameOver != null)
        {
            CanvasGameOver.SetActive(true); // Activar el canvas por seguridad
        }

        // Asegúrate de que los botones están correctamente referenciados
        if (ButtonReintentar != null)
        {
            ButtonReintentar.onClick.AddListener(ReloadScene); // Asignar función al botón Reintentar
        }
        else
        {
            Debug.LogError("ButtonReintentar no está asignado en el Inspector.");
        }

        if (ButtonSalir != null)
        {
            ButtonSalir.onClick.AddListener(GoToMenuNiveles); // Asignar función al botón Salir
        }
        else
        {
            Debug.LogError("ButtonSalir no está asignado en el Inspector.");
        }
    }

    // Método para reiniciar la escena actual
    private void ReloadScene()
    {
        Debug.Log("Botón Reintentar presionado"); // Mensaje para confirmar interacción

        // Asegurarse de que el tiempo está en escala normal
        Time.timeScale = 1f;

        // Obtener el nombre de la escena actual
        string currentScene = SceneManager.GetActiveScene().name;

        // Cargar nuevamente la misma escena
        SceneManager.LoadScene(currentScene);
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
