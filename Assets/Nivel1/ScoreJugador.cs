using System.IO;
using UnityEngine;
using TMPro; // Importar la librería de TextMesh Pro
using Newtonsoft.Json;

public class ScoreJugador : MonoBehaviour
{
    private Player currentPlayer; // Jugador actual
    public TMP_Text TextScore;    // Referencia al componente TextMesh Pro para mostrar el score
    private string dataPath = "Assets/Data/players.json"; // Ruta al archivo JSON

    void Start()
    {
        // Cargar los datos del jugador actual
        LoadCurrentPlayer();

        // Actualizar la interfaz de usuario con el score inicial
        UpdateScoreUI();
    }

    void Update()
    {
        // Cargar dinámicamente los datos actualizados del archivo JSON
        LoadCurrentPlayer();

        // Actualizar la interfaz de usuario con el score actualizado
        UpdateScoreUI();
    }

    // Método para cargar los datos del jugador actual
    private void LoadCurrentPlayer()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            var players = JsonConvert.DeserializeObject<Player[]>(json);

            // Buscar al jugador actual (por ejemplo, el que está en nivel1)
            if (players != null)
            {
                currentPlayer = System.Array.Find(players, player => player.nivel1);
            }
        }
        else
        {
            Debug.LogError($"El archivo JSON no existe en la ruta: {dataPath}");
        }
    }

    // Método para actualizar la interfaz de usuario con el score
    private void UpdateScoreUI()
    {
        if (currentPlayer != null)
        {
            int score = currentPlayer.score;

            // Formatear el score para que tenga 3 dígitos (Ejemplo: 000, 080, 999)
            TextScore.text = "Score: " + score.ToString("000"); 
        }
    }
}