using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    public Button botonPersonaje1;
    public Button botonPersonaje2;

    private const string FilePath = "Assets/Data/JugadorActual.json"; // Asegúrate de que la ruta es correcta
    private const string PlayersFilePath = "Assets/Data/players.json"; // Ruta para el archivo de todos los jugadores
    private List<Player> playersList = new List<Player>();
    private Player currentPlayer;

    void Start()
    {
        botonPersonaje1.onClick.AddListener(SelectPersonaje1);
        botonPersonaje2.onClick.AddListener(SelectPersonaje2);

        LoadPlayers();
    }

    void SelectPersonaje1()
    {
        Debug.Log("Seleccionado Personaje 1");

        if (currentPlayer != null)
        {
            currentPlayer.personajeAsset = "player1";  // Actualizar personaje
            SavePlayer();  // Guardar los cambios en el jugador actual
            UpdatePlayersList(); // Actualizar la lista de jugadores
        }
        else
        {
            Debug.LogWarning("No se ha encontrado un jugador para actualizar.");
        }

        SceneManager.LoadScene("MenuNiveles");
    }

    void SelectPersonaje2()
    {
        Debug.Log("Seleccionado Personaje 2");

        if (currentPlayer != null)
        {
            currentPlayer.personajeAsset = "player2";  // Actualizar personaje
            SavePlayer();  // Guardar los cambios en el jugador actual
            UpdatePlayersList(); // Actualizar la lista de jugadores
        }
        else
        {
            Debug.LogWarning("No se ha encontrado un jugador para actualizar.");
        }

        SceneManager.LoadScene("MenuNiveles");
    }

    void LoadPlayers()
    {
        // Intentar cargar los jugadores desde el archivo `JugadorActual.json`
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);

                // Deserializar como un solo jugador en lugar de una lista
                currentPlayer = JsonConvert.DeserializeObject<Player>(json);

                if (currentPlayer != null)
                {
                    Debug.Log($"Jugador cargado: {currentPlayer.nombre}");
                }
                else
                {
                    Debug.LogWarning("No se encontró un jugador en el archivo.");
                    // Crear un jugador si no existe ninguno
                    currentPlayer = new Player { id = "1", nombre = "Nuevo Jugador", personajeAsset = "player1", score = 0 };
                    SavePlayer();  // Guardar el jugador nuevo en el archivo
                }

                // Cargar todos los jugadores desde `players.json` si es necesario
                if (File.Exists(PlayersFilePath))
                {
                    string playersJson = File.ReadAllText(PlayersFilePath);
                    playersList = JsonConvert.DeserializeObject<List<Player>>(playersJson) ?? new List<Player>();
                }
                else
                {
                    Debug.LogWarning("El archivo de jugadores no existe. Creando uno nuevo.");
                    playersList.Add(currentPlayer);
                    SavePlayers();  // Guardar la lista de jugadores
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("El archivo de jugador actual no existe. Creando uno nuevo.");
            // Si el archivo no existe, se crea uno nuevo
            currentPlayer = new Player { id = "1", nombre = "Nuevo Jugador", personajeAsset = "player1", score = 0 };
            SavePlayer();  // Guardar el jugador nuevo
        }
    }

    void SavePlayer()
    {
        try
        {
            // Asegúrate de serializar como un solo jugador en `JugadorActual.json`
            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
            string directoryPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Depuración: Verifica el contenido JSON antes de escribirlo en el archivo
            Debug.Log($"Guardando jugador actual en el archivo JSON: {json}");

            File.WriteAllText(FilePath, json);
            Debug.Log("Jugador actual guardado correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar los datos del archivo JSON: " + ex.Message);
        }
    }

    void SavePlayers()
    {
        try
        {
            // Asegúrate de que todos los jugadores se están guardando en `players.json`
            string json = JsonConvert.SerializeObject(playersList, Formatting.Indented);
            string directoryPath = Path.GetDirectoryName(PlayersFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Depuración: Verifica el contenido JSON antes de escribirlo en el archivo
            Debug.Log($"Guardando datos en el archivo JSON: {json}");

            File.WriteAllText(PlayersFilePath, json);
            Debug.Log("Datos guardados correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar los datos en el archivo JSON: " + ex.Message);
        }
    }

    // Actualiza la lista de jugadores para reflejar el cambio en `players.json`
    void UpdatePlayersList()
    {
        if (currentPlayer != null)
        {
            // Actualizar la lista de jugadores con el jugador actual
            Player existingPlayer = playersList.Find(p => p.id == currentPlayer.id);
            if (existingPlayer != null)
            {
                existingPlayer.personajeAsset = currentPlayer.personajeAsset;
            }
            else
            {
                playersList.Add(currentPlayer); // Si no existe el jugador, agregarlo
            }

            SavePlayers();  // Guardar la lista actualizada de jugadores
        }
    }
}
