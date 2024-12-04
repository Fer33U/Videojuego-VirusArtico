using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic; // Para listas

public class SortPlayersByScore : MonoBehaviour
{
    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo único
    private List<Player> playersList = new List<Player>(); // Lista para almacenar los jugadores

    void Start()
    {
        // Cargar y ordenar los jugadores al iniciar el juego
        LoadAndSortPlayers();
    }

    void LoadAndSortPlayers()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                // Leer y deserializar el archivo JSON
                string json = File.ReadAllText(FilePath);
                playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                // Ordenar la lista de jugadores por 'score' de mayor a menor
                playersList.Sort((p1, p2) => p2.score.CompareTo(p1.score));

                Debug.Log("Datos ordenados correctamente.");

                // Guardar los datos ordenados en el archivo JSON
                SavePlayers();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar y ordenar los datos del archivo JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("El archivo players.json no existe.");
        }
    }

    void SavePlayers()
    {
        try
        {
            // Serializar la lista de jugadores a JSON
            string json = JsonConvert.SerializeObject(playersList, Formatting.Indented);

            // Guardar en el archivo
            File.WriteAllText(FilePath, json);

            Debug.Log("Datos ordenados y guardados correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar los datos en el archivo JSON: " + ex.Message);
        }
    }
}