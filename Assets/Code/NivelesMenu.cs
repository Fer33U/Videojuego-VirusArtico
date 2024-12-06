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

    private const string CurrentPlayerPath = "Assets/Data/JugadorActual.json"; // Ruta al archivo del jugador actual
    private const string PlayersPath = "Assets/Data/players.json"; // Ruta al archivo de todos los jugadores

    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Cargar los datos del jugador actual
        LoadCurrentPlayer();

        // Configurar la interfaz del nivel 2
        UpdateLevel2Assets();
    }

    public void LoadNivel1()
    {
        Debug.Log("Clic en el botón de Nivel 1");

        if (currentPlayer != null)
        {
            // Actualizar el valor de vidajugador a 3
            currentPlayer.vidajugador = 3;

            // Guardar los datos actualizados del jugador
            SaveCurrentPlayer();
            Debug.Log("Jugador actualizado con vidajugador = 3 antes de cargar Nivel1.");
        }

        // Cambiar a la escena Nivel1
        SceneManager.LoadScene("Nivel1");
    }

    public void LoadNivel2()
    {
        Debug.Log("Clic en el botón de Nivel 2");

        if (currentPlayer != null)
        {
            // Actualizar el valor de vidajugador a 3
            currentPlayer.vidajugador = 3;

            // Guardar los datos actualizados del jugador
            SaveCurrentPlayer();
            Debug.Log("Jugador actualizado con vidajugador = 3 antes de cargar Nivel2.");
        }

        // Cambiar a la escena Nivel2
        SceneManager.LoadScene("Nivel2");
    }

    private void LoadCurrentPlayer()
    {
        // Cargar el jugador actual desde el archivo JugadorActual.json
        if (File.Exists(CurrentPlayerPath))
        {
            try
            {
                string json = File.ReadAllText(CurrentPlayerPath);
                currentPlayer = JsonConvert.DeserializeObject<Player>(json);

                if (currentPlayer != null)
                {
                    Debug.Log($"Jugador actual cargado: {currentPlayer.nombre}");
                }
                else
                {
                    Debug.LogWarning("No se pudo deserializar el jugador actual.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al cargar el jugador actual: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"El archivo {CurrentPlayerPath} no existe.");
        }
    }

    private void UpdateLevel2Assets()
    {
        if (currentPlayer != null)
        {
            if (currentPlayer.nivel1)
            {
                // Si el nivel 1 está completado, habilitar el nivel 2
                nivel2CompletoAsset.SetActive(true);
                nivel2SinCompletarAsset.SetActive(false);
                botonNivel2.interactable = true;
            }
            else
            {
                // Si el nivel 1 no está completado, deshabilitar el nivel 2
                nivel2CompletoAsset.SetActive(false);
                nivel2SinCompletarAsset.SetActive(true);
                botonNivel2.interactable = false;
            }
        }
        else
        {
            nivel2CompletoAsset.SetActive(false);
            nivel2SinCompletarAsset.SetActive(true);
            botonNivel2.interactable = false;
        }
    }

    private void SaveCurrentPlayer()
    {
        if (currentPlayer != null)
        {
            try
            {
                // Guardar el jugador actual en JugadorActual.json
                string currentPlayerJson = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
                File.WriteAllText(CurrentPlayerPath, currentPlayerJson);

                Debug.Log("Datos del jugador actual guardados correctamente.");

                // También actualizar el archivo players.json
                SavePlayerInPlayersFile();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al guardar el jugador actual: {ex.Message}");
            }
        }
    }

    private void SavePlayerInPlayersFile()
    {
        try
        {
            // Leer la lista de jugadores existente
            List<Player> playersList = new List<Player>();

            if (File.Exists(PlayersPath))
            {
                string playersJson = File.ReadAllText(PlayersPath);
                playersList = JsonConvert.DeserializeObject<List<Player>>(playersJson) ?? new List<Player>();
            }

            // Buscar al jugador actual en la lista
            bool playerFound = false;

            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].id == currentPlayer.id)
                {
                    playersList[i] = currentPlayer; // Actualizar el jugador
                    playerFound = true;
                    break;
                }
            }

            // Si el jugador no estaba en la lista, agregarlo
            if (!playerFound)
            {
                playersList.Add(currentPlayer);
            }

            // Guardar la lista actualizada en players.json
            string updatedPlayersJson = JsonConvert.SerializeObject(playersList, Formatting.Indented);
            File.WriteAllText(PlayersPath, updatedPlayersJson);

            Debug.Log("Lista de jugadores actualizada correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al actualizar la lista de jugadores: {ex.Message}");
        }
    }
}
