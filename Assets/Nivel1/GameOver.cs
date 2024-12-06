using UnityEngine;
using UnityEngine.SceneManagement; // Importar SceneManagement para cargar escenas
using UnityEngine.UI; // Importar UI para trabajar con botones
using System.IO; // Importar para trabajar con archivos
using Newtonsoft.Json; // Importar para trabajar con JSON

public class GameOverScript : MonoBehaviour
{
    public Button ButtonReintentar; // Referencia al botón de reiniciar
    public Button ButtonSalir;      // Referencia al botón de salir
    public GameObject CanvasGameOver; // Referencia al Canvas del GameOver

    private string filePath = "Assets/Data/players.json"; // Ruta del archivo JSON
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Asegúrate de que los botones están conectados y asignarles las funciones correspondientes
        ButtonReintentar.onClick.AddListener(OnReintentarClicked); // Actualizar datos y reiniciar escena
        ButtonSalir.onClick.AddListener(GoToMenuNiveles);   // Ir al menú de niveles

        // Cargar los datos del jugador actual
        LoadCurrentPlayer();
    }

    // Método para cargar los datos del jugador actual
    private void LoadCurrentPlayer()
    {
        // Leer el archivo JSON
        string json = File.ReadAllText(filePath);

        // Convertir el JSON a una lista de jugadores
        Player[] players = JsonConvert.DeserializeObject<Player[]>(json);

        // Encontrar al jugador actual (por nombre o cualquier otro criterio)
        // Aquí, puedes cambiar el criterio de búsqueda según tus necesidades
        currentPlayer = System.Array.Find(players, player => player.nombre == "Fernando");

        if (currentPlayer != null)
        {
            Debug.Log("Jugador cargado: " + currentPlayer.nombre);
        }
        else
        {
            Debug.LogError("Jugador no encontrado");
        }
    }

    // Método para actualizar los datos del jugador y reiniciar la escena
    private void OnReintentarClicked()
    {
        if (currentPlayer != null)
        {
            // Actualizar el valor de vidajugador a 3
            currentPlayer.vidajugador = 3;

            // Leer el archivo JSON
            string json = File.ReadAllText(filePath);

            // Convertir el JSON a una lista de jugadores
            Player[] players = JsonConvert.DeserializeObject<Player[]>(json);

            // Buscar y actualizar el jugador en la lista
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].nombre == currentPlayer.nombre)
                {
                    players[i] = currentPlayer;
                    break;
                }
            }

            // Convertir nuevamente la lista de jugadores a JSON
            string updatedJson = JsonConvert.SerializeObject(players, Formatting.Indented);

            // Guardar el archivo JSON actualizado
            File.WriteAllText(filePath, updatedJson);

            Debug.Log("Datos del jugador actualizados");
        }

        // Llamar a la función para reiniciar la escena después de actualizar los datos
        ReloadScene();
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
    private void GoToMenuNiveles()
    {
        // Cargar la escena del menú de niveles
        SceneManager.LoadScene("MenuNiveles"); // Asegúrate de que el nombre de la escena sea correcto
    }
}