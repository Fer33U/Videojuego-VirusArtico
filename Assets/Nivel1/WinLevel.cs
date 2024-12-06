using UnityEngine;
using UnityEngine.SceneManagement; // Importar para manejar las escenas
using UnityEngine.UI; // Importar para trabajar con UI
using System.IO; // Importar para trabajar con archivos
using Newtonsoft.Json;
using System.Collections.Generic; // Importar para trabajar con JSON

public class WinLevel : MonoBehaviour
{
    public Button ButtonSiguienteNivel; // Referencia al botón de siguiente nivel
    public Button ButtonSalir;          // Referencia al botón de salir

    private string playersFilePath = "Assets/Data/players.json"; // Ruta del archivo JSON de jugadores
    private string currentPlayerFilePath = "Assets/Data/JugadorActual.json"; // Ruta del archivo JSON del jugador actual
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Asegúrate de que los botones están conectados a sus respectivas funciones
        ButtonSiguienteNivel.onClick.AddListener(OnSiguienteNivelClicked); // Cambiar a Nivel2 y actualizar datos
        ButtonSalir.onClick.AddListener(GoToMenuNiveles);     // Volver al menú de niveles

        // Cargar los datos del jugador actual
        LoadCurrentPlayer();
    }

    // Método para cargar los datos del jugador actual desde JugadorActual.json
    private void LoadCurrentPlayer()
    {
        if (File.Exists(currentPlayerFilePath))
        {
            try
            {
                // Leer el archivo JSON
                string json = File.ReadAllText(currentPlayerFilePath);

                // Convertir el JSON al jugador actual
                currentPlayer = JsonConvert.DeserializeObject<Player>(json);

                if (currentPlayer != null)
                {
                    Debug.Log("Jugador cargado: " + currentPlayer.nombre);
                }
                else
                {
                    Debug.LogError("Jugador actual no encontrado en el archivo JSON.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar el jugador actual: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("El archivo JugadorActual.json no existe.");
        }
    }

    // Método para actualizar los datos del jugador y cambiar de nivel
    private void OnSiguienteNivelClicked()
    {
        if (currentPlayer != null)
        {
            // Actualizar el valor de vidajugador a 3
            currentPlayer.vidajugador = 3;

            // Guardar los cambios en JugadorActual.json
            SaveCurrentPlayer();

            // Actualizar también los datos en players.json
            UpdatePlayerInPlayersFile();

            Debug.Log("Datos del jugador actualizados.");
        }

        // Llamar al método para ir al siguiente nivel
        GoToNivel2();
    }

    // Método para guardar los datos del jugador actual en JugadorActual.json
    private void SaveCurrentPlayer()
    {
        try
        {
            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);

            // Crear la carpeta si no existe
            string directoryPath = Path.GetDirectoryName(currentPlayerFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Escribir en el archivo JSON
            File.WriteAllText(currentPlayerFilePath, json);
            Debug.Log("Jugador actual guardado correctamente en JugadorActual.json.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar el jugador actual: " + ex.Message);
        }
    }

    // Método para actualizar los datos del jugador en players.json
    private void UpdatePlayerInPlayersFile()
    {
        if (File.Exists(playersFilePath))
        {
            try
            {
                // Leer el archivo JSON
                string json = File.ReadAllText(playersFilePath);

                // Convertir el JSON a una lista de jugadores
                Player[] players = JsonConvert.DeserializeObject<Player[]>(json);

                // Buscar y actualizar al jugador actual en la lista
                bool playerFound = false;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].id == currentPlayer.id)
                    {
                        players[i] = currentPlayer;
                        playerFound = true;
                        break;
                    }
                }

                // Si el jugador no está en la lista, agregarlo
                if (!playerFound)
                {
                    var playersList = new List<Player>(players) { currentPlayer };
                    players = playersList.ToArray();
                }

                // Guardar los cambios en players.json
                string updatedJson = JsonConvert.SerializeObject(players, Formatting.Indented);
                File.WriteAllText(playersFilePath, updatedJson);

                Debug.Log("Datos actualizados en players.json.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al actualizar players.json: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("El archivo players.json no existe.");
        }
    }

    // Método para ir a la escena Nivel2
    private void GoToNivel2()
    {
        // Cargar la escena Nivel2
        SceneManager.LoadScene("Nivel2"); // Asegúrate de que el nombre de la escena sea correcto
    }

    // Método para ir al menú de niveles
 // Método para ir al menú de niveles
private void GoToMenuNiveles()
{
    // Asegurarse de que el jugador actual esté cargado antes de modificarlo
    if (currentPlayer != null)
    {
        // Actualizar el valor de vidajugador a 3
        currentPlayer.vidajugador = 3;

        // Guardar los datos actualizados del jugador actual en JugadorActual.json
        SaveCurrentPlayer();

        // Actualizar los datos del jugador en players.json
        UpdatePlayerInPlayersFile();

        Debug.Log("Valor de vidajugador actualizado a 3.");
    }

    // Cargar la escena del menú de niveles
    SceneManager.LoadScene("MenuNiveles"); // Asegúrate de que el nombre de la escena sea correcto
}

}