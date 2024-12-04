using UnityEngine;
using UnityEngine.UI;  // Para trabajar con botones
using UnityEngine.SceneManagement;  // Para cargar escenas
using TMPro;  // Para TextMeshPro (si lo usas para los botones o entradas de texto)
using System.IO; // Para manipulación de archivos
using Newtonsoft.Json; // Para serializar objetos a JSON
using System.Collections.Generic; // Para listas

public class CharacterSelector : MonoBehaviour
{
    public Button botonPersonaje1; // Botón para seleccionar el personaje 1
    public Button botonPersonaje2; // Botón para seleccionar el personaje 2

    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo de jugadores
    private List<Player> playersList = new List<Player>(); // Lista de jugadores
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Configurar los botones con sus funciones
        botonPersonaje1.onClick.AddListener(SelectPersonaje1);
        botonPersonaje2.onClick.AddListener(SelectPersonaje2);

        // Cargar jugadores existentes
        LoadPlayers();
    }

    // Función para seleccionar el personaje 1
    void SelectPersonaje1()
    {
        Debug.Log("Seleccionado Personaje 1");

        // Actualizar el campo 'personajeAsset' del jugador actual
        if (currentPlayer != null)
        {
            currentPlayer.personajeAsset = "player1";  // Asignar 'player1' al personajeAsset
            SavePlayers(); // Guardar los cambios
        }

        // Redirigir a la escena 'MenuNiveles'
        SceneManager.LoadScene("MenuNiveles");
    }

    // Función para seleccionar el personaje 2
    void SelectPersonaje2()
    {
        Debug.Log("Seleccionado Personaje 2");

        // Actualizar el campo 'personajeAsset' del jugador actual
        if (currentPlayer != null)
        {
            currentPlayer.personajeAsset = "player2";  // Asignar 'player2' al personajeAsset
            SavePlayers(); // Guardar los cambios
        }

        // Redirigir a la escena 'MenuNiveles'
        SceneManager.LoadScene("MenuNiveles");
    }

    // Función para cargar los datos de los jugadores desde el archivo JSON
    void LoadPlayers()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                // Obtener el jugador actual (último jugador registrado)
                if (playersList.Count > 0)
                {
                    currentPlayer = playersList[playersList.Count - 1];
                    Debug.Log($"Jugador cargado: {currentPlayer.nombre}");
                }
                else
                {
                    Debug.LogWarning("No se encontró ningún jugador en el archivo.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("El archivo de jugadores no existe.");
        }
    }

    // Función para guardar los datos de los jugadores en el archivo JSON
    void SavePlayers()
    {
        try
        {
            string json = JsonConvert.SerializeObject(playersList, Formatting.Indented);
            string directoryPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(FilePath, json);
            Debug.Log("Datos guardados correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar los datos en el archivo JSON: " + ex.Message);
        }
    }
}
