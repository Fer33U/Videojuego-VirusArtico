using UnityEngine;
using UnityEngine.SceneManagement; // Importar para manejar las escenas
using UnityEngine.UI; // Importar para trabajar con UI
using System.IO; // Importar para trabajar con archivos
using Newtonsoft.Json;
using System.Collections.Generic; // Importar para trabajar con JSON

public class WinLevel : MonoBehaviour
{
    public Button ButtonSiguienteNivel;
    public Button ButtonSalir;
    public GameObject canvasWin; // Referencia al canvas que aparece cuando el jugador gana

    private string playersFilePath = "Assets/Data/players.json";
    private string currentPlayerFilePath = "Assets/Data/JugadorActual.json";
    private Player currentPlayer;

    void Start()
    {
        ButtonSiguienteNivel.onClick.AddListener(OnSiguienteNivelClicked);
        ButtonSalir.onClick.AddListener(GoToMenuNiveles);

        LoadCurrentPlayer();

        // Asegurarse de detectar la aparición del canvasWin
        if (canvasWin != null)
        {
            canvasWin.SetActive(false); // Asegúrate de que inicialmente esté desactivado
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

      // Método para ir a la escena Nivel2
    private void GoToNivel2()
    {
        // Cargar la escena Nivel2
        SceneManager.LoadScene("Nivel2"); // Asegúrate de que el nombre de la escena sea correcto
    }

    void Update()
    {
        // Si el canvasWin está activo, actualiza los datos
        if (canvasWin != null && canvasWin.activeSelf)
        {
            UpdateLevelData();
            canvasWin = null; // Asegurarse de que no se llame repetidamente
        }
    }

    private void UpdateLevelData()
    {
        if (currentPlayer != null)
        {
            currentPlayer.nivel1 = true; // Cambiar el nivel1 a true
            SaveCurrentPlayer(); // Guardar en JugadorActual.json
            UpdatePlayerInPlayersFile(); // Actualizar en players.json
            Debug.Log("Nivel 1 actualizado a true para el jugador actual.");
        }
        else
        {
            Debug.LogError("No se pudo actualizar el nivel1 porque el jugador actual no está cargado.");
        }
    }

    private void LoadCurrentPlayer()
    {
        if (File.Exists(currentPlayerFilePath))
        {
            try
            {
                string json = File.ReadAllText(currentPlayerFilePath);
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

    private void SaveCurrentPlayer()
    {
        try
        {
            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
            string directoryPath = Path.GetDirectoryName(currentPlayerFilePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(currentPlayerFilePath, json);
            Debug.Log("Jugador actual guardado correctamente en JugadorActual.json.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar el jugador actual: " + ex.Message);
        }
    }

    private void UpdatePlayerInPlayersFile()
    {
        if (File.Exists(playersFilePath))
        {
            try
            {
                string json = File.ReadAllText(playersFilePath);
                Player[] players = JsonConvert.DeserializeObject<Player[]>(json);
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

                if (!playerFound)
                {
                    var playersList = new List<Player>(players) { currentPlayer };
                    players = playersList.ToArray();
                }

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

    private void GoToMenuNiveles()
    {
        if (currentPlayer != null)
        {
            currentPlayer.vidajugador = 3;
            SaveCurrentPlayer();
            UpdatePlayerInPlayersFile();
        }
        SceneManager.LoadScene("MenuNiveles");
    }
}
