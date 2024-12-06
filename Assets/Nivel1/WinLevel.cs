using UnityEngine;
using UnityEngine.SceneManagement; // Importar para manejar las escenas
using UnityEngine.UI; // Importar para trabajar con UI
using System.IO; // Importar para trabajar con archivos
using Newtonsoft.Json; // Importar para trabajar con JSON

public class WinLevel : MonoBehaviour
{
    public Button ButtonSiguienteNivel; // Referencia al botón de siguiente nivel
    public Button ButtonSalir;          // Referencia al botón de salir

    private string filePath = "Assets/Data/players.json"; // Ruta del archivo JSON
    private Player currentPlayer; // Jugador actual

    void Start()
    {
        // Asegúrate de que los botones están conectados a sus respectivas funciones
        ButtonSiguienteNivel.onClick.AddListener(OnSiguienteNivelClicked); // Cambiar a Nivel2 y actualizar datos
        ButtonSalir.onClick.AddListener(GoToMenuNiveles);     // Volver al menú de niveles

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

    // Método para actualizar los datos del jugador y cambiar de nivel
    private void OnSiguienteNivelClicked()
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

        // Llamar al método para ir al siguiente nivel
        GoToNivel2();
    }

    // Método para ir a la escena Nivel2
    private void GoToNivel2()
    {
        // Cargar la escena Nivel2
        SceneManager.LoadScene("Nivel2"); // Asegúrate de que el nombre de la escena sea correcto
    }

    // Método para ir al menú de niveles
    private void GoToMenuNiveles()
    {
        // Cargar la escena MenuNiveles
        SceneManager.LoadScene("MenuNiveles"); // Asegúrate de que el nombre de la escena sea correcto
    }
}
