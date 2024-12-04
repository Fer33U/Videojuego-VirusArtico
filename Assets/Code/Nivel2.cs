using UnityEngine;
using System.IO; // Para manejo de archivos
using Newtonsoft.Json; // Para deserialización JSON
using System.Collections.Generic; // Para listas

public class Nivel2 : MonoBehaviour
{
    public GameObject personaje1_1; // Asigna en el Inspector el GameObject del primer personaje
    public GameObject personaje2_0; // Asigna en el Inspector el GameObject del segundo personaje

    private Player currentPlayer; // Jugador actual obtenido desde Nivel1Utilities

    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo JSON

    void Start()
    {
        // Validar que los objetos estén asignados
        if (personaje1_1 == null || personaje2_0 == null)
        {
            Debug.LogError("Los GameObjects 'personaje1_1' o 'personaje2_0' no están asignados en el Inspector.");
            return;
        }

        // Obtener el jugador actual desde el archivo JSON o el sistema compartido
        LoadPlayersFromJson();

        // Configurar personajes en el nivel 2 según el jugador actual
        ConfigureCharacters();
    }

    void LoadPlayersFromJson()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                List<Player> playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                // Seleccionamos el último jugador como el jugador actual
                currentPlayer = playersList.Count > 0 ? playersList[playersList.Count - 1] : null;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
                currentPlayer = null;
            }
        }
        else
        {
            Debug.LogWarning("El archivo players.json no existe.");
            currentPlayer = null;
        }
    }

    void ConfigureCharacters()
    {
        if (currentPlayer != null)
        {
            Debug.Log($"Jugador actual en Nivel2: {currentPlayer.nombre}, personajeAsset: {currentPlayer.personajeAsset}");

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
                ActivarPredeterminado();
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador actual en Nivel2.");
            ActivarPredeterminado();
        }
    }

    void ActivarPredeterminado()
    {
        Debug.Log("Activando configuración predeterminada en Nivel2.");
        personaje1_1.SetActive(true);
        personaje2_0.SetActive(false);
    }
}
