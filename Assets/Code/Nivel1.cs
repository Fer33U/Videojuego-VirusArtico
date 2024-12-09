using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic; // Para listas

public class Nivel1 : MonoBehaviour
{
    public GameObject personaje1_1; // Asigna en el Inspector el GameObject del primer personaje
    public GameObject personaje2_0; // Asigna en el Inspector el GameObject del segundo personaje

    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo de datos
    private List<Player> playersList = new List<Player>(); // Lista para almacenar los jugadores

    void Start()
    {
        Time.timeScale = 1;
        // Cargar jugadores y gestionar los personajes
        LoadPlayersAndSetCharacter();
    }

    void LoadPlayersAndSetCharacter()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                // Leer y deserializar el archivo JSON
                string json = File.ReadAllText(FilePath);
                playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                // Obtener el jugador actual (ajusta cómo seleccionas al jugador actual si es necesario)
                Player currentPlayer = GetCurrentPlayer();

                if (currentPlayer != null)
                {
                    // Activar/Desactivar personajes según el personajeAsset
                    if (currentPlayer.personajeAsset == "player1")
                    {
                        personaje1_1.SetActive(true);
                        personaje2_0.SetActive(false);
                    }
                    else if (currentPlayer.personajeAsset == "player2")
                    {
                        personaje1_1.SetActive(false);
                        personaje2_0.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("El personajeAsset del jugador no es válido.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el jugador actual.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("El archivo players.json no existe.");
        }
    }

    Player GetCurrentPlayer()
    {
        // Lógica para obtener el jugador actual
        // Aquí asumimos que el último jugador en la lista es el actual.
        if (playersList.Count > 0)
        {
            return playersList[playersList.Count - 1];
        }
        return null;
    }
}
