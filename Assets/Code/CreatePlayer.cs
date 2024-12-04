using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir el espacio de nombres de TextMeshPro
using System.IO;  // Para manipulación de archivos
using Newtonsoft.Json;  // Para serializar objetos a JSON
using System.Collections.Generic; // Para listas
using UnityEngine.SceneManagement; // Para cambiar de escena

public class CreatePlayer : MonoBehaviour
{
    public TMP_InputField playerNameInput; // Cambiado a TMP_InputField
    public Button submitButton;       // Asigna el botón desde el inspector

    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo único
    private List<Player> playersList = new List<Player>();
    private const int MaxPlayers = 15;

    private Player currentPlayer; // Referencia al jugador actual

    void Start()
    {
        // Configurar el evento del botón
        submitButton.onClick.AddListener(CreateNewPlayer);

        // Cargar datos existentes
        LoadPlayers();
    }

    void CreateNewPlayer()
    {
        // Obtener el nombre del jugador desde el input field
        string playerName = playerNameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("El nombre del jugador está vacío.");
            return;
        }

        // Crear el nuevo jugador
        Player newPlayer = new Player
        {
            nombre = playerName,
            score = 0,
            nivel1 = false,
            nivel2 = false,
            personajeAsset = "",  // Asignar una cadena vacía para el personajeAsset
            vidajugador = 0      // Inicializar vidajugador con 0 (vacío numérico)
        };

        // Agregar el nuevo jugador a la lista
        playersList.Add(newPlayer);

        // Mantener un máximo de 15 jugadores
        if (playersList.Count > MaxPlayers)
        {
            playersList.RemoveAt(0); // Eliminar el jugador más antiguo
        }

        // Guardar datos actualizados
        SavePlayers();

        // Establecer el jugador actual
        currentPlayer = newPlayer;

        Debug.Log($"Jugador {playerName} creado y guardado.");

        // Cambiar a la siguiente escena
        LoadNextScene();
    }

    void LoadPlayers()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                // Leer y deserializar el archivo JSON
                string json = File.ReadAllText(FilePath);
                playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                Debug.Log("Datos cargados correctamente.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
            }
        }
    }

    void SavePlayers()
    {
        try
        {
            // Serializar la lista de jugadores a JSON
            string json = JsonConvert.SerializeObject(playersList, Formatting.Indented);

            // Crear la carpeta si no existe
            string directoryPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Guardar en el archivo
            File.WriteAllText(FilePath, json);

            Debug.Log("Datos guardados correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar los datos en el archivo JSON: " + ex.Message);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}

// Clase que representa los datos del jugador
[System.Serializable]
public class Player
{
    public string nombre;
    public int score;
    public bool nivel1;
    public bool nivel2;
    public string personajeAsset; // Nuevo campo para el asset del personaje
    public int vidajugador; // Nuevo campo para la vida del jugador
}
