using UnityEngine;
using UnityEngine.SceneManagement; // Para cargar escenas
using UnityEngine.UI; // Para trabajar con botones
using System.IO; // Para trabajar con archivos
using Newtonsoft.Json; // Para trabajar con JSON

public class GameOverScript : MonoBehaviour
{
    public Button ButtonReintentar; // Referencia al botón de reiniciar
    public Button ButtonSalir;      // Referencia al botón de salir
    public GameObject CanvasGameOver; // Referencia al Canvas del GameOver

    private const string PlayersFilePath = "Assets/Data/players.json"; // Ruta del archivo de jugadores
    private const string CurrentPlayerFilePath = "Assets/Data/JugadorActual.json"; // Ruta del archivo del jugador actual
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Asegúrate de que los botones están conectados y asignarles las funciones correspondientes
        ButtonReintentar.onClick.AddListener(OnReintentarClicked); // Actualizar datos y reiniciar escena
        ButtonSalir.onClick.AddListener(GoToMenuNiveles);   // Ir al menú de niveles

        // Cargar los datos del jugador actual
        LoadCurrentPlayer();
    }

    // Método para cargar los datos del jugador actual desde JugadorActual.json
    private void LoadCurrentPlayer()
    {
        if (File.Exists(CurrentPlayerFilePath))
        {
            try
            {
                string json = File.ReadAllText(CurrentPlayerFilePath);
                currentPlayer = JsonConvert.DeserializeObject<Player>(json);

                if (currentPlayer != null)
                {
                    Debug.Log("Jugador cargado desde JugadorActual.json: " + currentPlayer.nombre);
                }
                else
                {
                    Debug.LogError("No se pudo deserializar el jugador actual.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar JugadorActual.json: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Archivo JugadorActual.json no encontrado.");
        }
    }

    // Método para actualizar los datos del jugador y reiniciar la escena
    private void OnReintentarClicked()
    {
        if (currentPlayer != null)
        {
            // Actualizar el valor de vidajugador a 3
            currentPlayer.vidajugador = 3;

            // Actualizar JugadorActual.json
            SaveCurrentPlayer();

            // Actualizar players.json
            UpdatePlayerInList();

            Debug.Log("Datos del jugador actualizados.");
        }

        // Llamar a la función para reiniciar la escena después de actualizar los datos
        ReloadScene();
    }

    // Método para guardar los datos actualizados del jugador actual en JugadorActual.json
    private void SaveCurrentPlayer()
    {
        try
        {
            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);

            File.WriteAllText(CurrentPlayerFilePath, json);

            Debug.Log("JugadorActual.json actualizado.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al actualizar JugadorActual.json: " + ex.Message);
        }
    }

    // Método para actualizar los datos del jugador actual en players.json
    private void UpdatePlayerInList()
    {
        if (File.Exists(PlayersFilePath))
        {
            try
            {
                string json = File.ReadAllText(PlayersFilePath);
                Player[] players = JsonConvert.DeserializeObject<Player[]>(json);

                // Buscar y actualizar al jugador en la lista
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].id == currentPlayer.id) // Comparar por ID único
                    {
                        players[i] = currentPlayer;
                        break;
                    }
                }

                // Guardar la lista actualizada en players.json
                string updatedJson = JsonConvert.SerializeObject(players, Formatting.Indented);
                File.WriteAllText(PlayersFilePath, updatedJson);

                Debug.Log("Players.json actualizado.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al actualizar Players.json: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Archivo Players.json no encontrado.");
        }
    }

    // Método para reiniciar la escena actual
    private void ReloadScene()
    {
        // Desactivar el Canvas del GameOver
        CanvasGameOver.SetActive(false);

        // Obtener el nombre de la escena actual
        string currentScene = SceneManager.GetActiveScene().name;

        // Cargar nuevamente la misma escena
        SceneManager.LoadScene(currentScene);
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
        UpdatePlayerInList();

        Debug.Log("Valor de vidajugador actualizado a 3.");
    }

    // Cargar la escena del menú de niveles
    SceneManager.LoadScene("MenuNiveles"); // Asegúrate de que el nombre de la escena sea correcto
}

}