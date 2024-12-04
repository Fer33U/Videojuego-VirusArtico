using UnityEngine;
using UnityEngine.UI;
using System.IO; // Para manipulación de archivos
using Newtonsoft.Json; // Para serializar y deserializar JSON
using System.Collections.Generic; // Para listas
using UnityEngine.SceneManagement; // Para cambiar de escenas

public class LevelSelector : MonoBehaviour
{
    public GameObject nivel2CompletoAsset; // Asset para nivel 2 completado
    public GameObject nivel2SinCompletarAsset; // Asset para nivel 2 sin completar
    public Button botonNivel1; // Botón para el nivel 1
    public Button botonNivel2; // Botón para el nivel 2

    private const string FilePath = "Assets/Data/players.json"; // Ruta al archivo JSON
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Cargar los datos del jugador actual
        LoadCurrentPlayer();

        // Configurar la interfaz del nivel 2
        UpdateLevel2Assets();
    }

    // Función pública para cargar la escena Nivel1
    public void LoadNivel1()
    {
        Debug.Log("Clic en el botón de Nivel 1"); // Log cuando se hace clic en el botón de Nivel 1
        SceneManager.LoadScene("Nivel1");
    }

    // Función pública para cargar la escena Nivel2
    public void LoadNivel2()
    {
        Debug.Log("Clic en el botón de Nivel 2"); // Log cuando se hace clic en el botón de Nivel 2
        SceneManager.LoadScene("Nivel2");
    }

    void LoadCurrentPlayer()
    {
        // Asegurarse de que el archivo JSON exista
        if (File.Exists(FilePath))
        {
            try
            {
                // Leer y deserializar la lista de jugadores
                string json = File.ReadAllText(FilePath);
                List<Player> playersList = JsonConvert.DeserializeObject<List<Player>>(json);

                // Obtener al jugador actual (en este caso, el último registrado)
                if (playersList != null && playersList.Count > 0)
                {
                    currentPlayer = playersList[playersList.Count - 1];
                    Debug.Log($"Jugador actual cargado: {currentPlayer.nombre}");
                }
                else
                {
                    Debug.LogWarning("No se encontró ningún jugador en el archivo.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al cargar los datos del archivo JSON: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("El archivo de jugadores no existe.");
        }
    }

    void UpdateLevel2Assets()
    {
        if (currentPlayer != null)
        {
            if (currentPlayer.nivel1)
            {
                // Si el nivel 1 está completado, habilitar el nivel 2
                nivel2CompletoAsset.SetActive(true); // Mostrar el asset del nivel 2 completado
                nivel2SinCompletarAsset.SetActive(false); // Ocultar el asset de nivel 2 sin completar
                botonNivel2.interactable = true; // Habilitar el botón de nivel 2
            }
            else
            {
                // Si el nivel 1 no está completado, deshabilitar el nivel 2
                nivel2CompletoAsset.SetActive(false); // Ocultar el asset del nivel 2 completado
                nivel2SinCompletarAsset.SetActive(true); // Mostrar el asset de nivel 2 sin completar
                botonNivel2.interactable = false; // Deshabilitar el botón de nivel 2
            }
        }
        else
        {
            // Si no hay jugador cargado, deshabilitar el acceso al nivel 2
            nivel2CompletoAsset.SetActive(false);
            nivel2SinCompletarAsset.SetActive(true);
            botonNivel2.interactable = false;
        }
    }
}
