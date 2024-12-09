using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ZombieCode : MonoBehaviour
{
    public float speed = 2f;  // Velocidad del zombie
    public GameObject personaje1_1; // Referencia al primer personaje
    public GameObject personaje2_0; // Referencia al segundo personaje
    private Transform player; // Referencia al jugador activo
    private Animator zombieAnimator; // Animator del zombie
    private Rigidbody2D rb; // Rigidbody2D del zombie
    private float lastDamageTime = 0f; // Tiempo del último daño
    private float damageCooldown = 3f; // Tiempo de espera entre daños (en segundos)
    public AudioSource audioDano; // Referencia al componente AudioSource para el sonido de daño

    // Ruta de los archivos JSON
    private string jugadorActualPath = "Assets/Data/JugadorActual.json";
    private string playersPath = "Assets/Data/players.json";

    void Start()
    {
        // Llamar a la coroutine para esperar 2 segundos antes de buscar al jugador
        StartCoroutine(WaitForPlayer());
    }

    // Coroutine para esperar 2 segundos antes de buscar al jugador
    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(1f); // Espera 2 segundos

        // Buscar el jugador activo entre "personaje1_1" y "personaje2_0"
        if (personaje1_1 != null && personaje1_1.activeInHierarchy)
        {
            player = personaje1_1.transform;
            Debug.Log("Jugador activo: personaje1_1");
        }
        else if (personaje2_0 != null && personaje2_0.activeInHierarchy)
        {
            player = personaje2_0.transform;
            Debug.Log("Jugador activo: personaje2_0");
        }

        if (player == null)
        {
            Debug.LogError("No se encontró un jugador activo.");
        }

        // Obtener el componente Animator del zombie
        zombieAnimator = GetComponent<Animator>();
        if (zombieAnimator == null)
        {
            Debug.LogError("No se encontró un Animator en el objeto del zombie.");
        }

        // Obtener el componente Rigidbody2D del zombie
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody2D en el objeto del zombie.");
        }

        // Verificar que el AudioSource esté configurado
        if (audioDano == null)
        {
            Debug.LogError("No se encontró un AudioSource asignado al zombie para el sonido de daño.");
        }
    }

    void Update()
    {
        if (player != null && zombieAnimator != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;
            float moveX = direction.x;
            float moveY = direction.y;

            // Establecer los parámetros del Animator para el movimiento
            zombieAnimator.SetFloat("Horizontal", moveX);
            zombieAnimator.SetFloat("Vertical", moveY);
            zombieAnimator.SetFloat("Speed", direction.magnitude);

            // Mover al zombie hacia el jugador utilizando Rigidbody2D.MovePosition()
            Vector2 targetPosition = new Vector2(player.position.x, player.position.y);
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D llamado");

        // Verificar si el objeto con el que colisiona es el jugador activo
        if (Time.time > lastDamageTime + damageCooldown)
        {
            if (other.transform == player)
            {
                Debug.Log("Colisión con el jugador detectada");

                // Reproducir sonido de daño
                if (audioDano != null)
                {
                    audioDano.Play();
                }

                // Obtener el jugador actual y actualizar los datos
                if (File.Exists(jugadorActualPath))
                {
                    string jugadorActualJson = File.ReadAllText(jugadorActualPath);
                    Player jugadorActual = JsonConvert.DeserializeObject<Player>(jugadorActualJson);

                    if (jugadorActual != null)
                    {
                        Debug.Log("Jugador actual: " + jugadorActual.nombre + " vida: " + jugadorActual.vidajugador);

                        // Restar vida al jugador y actualizar el score
                        jugadorActual.vidajugador -= 1;
                        jugadorActual.score -= 10;

                        // Guardar los cambios en el archivo JugadorActual.json
                        string updatedJugadorJson = JsonConvert.SerializeObject(jugadorActual, Formatting.Indented);
                        File.WriteAllText(jugadorActualPath, updatedJugadorJson);

                        Debug.Log("Datos actualizados para " + jugadorActual.nombre + " vida: " + jugadorActual.vidajugador + " score: " + jugadorActual.score);

                        // Actualizar players.json si el jugador actual es el mismo
                        if (File.Exists(playersPath))
                        {
                            string playersJson = File.ReadAllText(playersPath);
                            List<Player> players = JsonConvert.DeserializeObject<List<Player>>(playersJson);

                            foreach (Player player in players)
                            {
                                if (player.id == jugadorActual.id)
                                {
                                    player.vidajugador = jugadorActual.vidajugador;
                                    player.score = jugadorActual.score;
                                    break;
                                }
                            }

                            // Guardar los cambios en players.json
                            string updatedPlayersJson = JsonConvert.SerializeObject(players, Formatting.Indented);
                            File.WriteAllText(playersPath, updatedPlayersJson);
                        }
                        else
                        {
                            Debug.LogError("No se encontró el archivo players.json");
                        }
                    }
                    else
                    {
                        Debug.LogError("El jugador actual no se pudo deserializar.");
                    }
                }
                else
                {
                    Debug.LogError("No se encontró el archivo JugadorActual.json");
                }

                // Actualizar el tiempo del último daño
                lastDamageTime = Time.time;
            }
            else
            {
                Debug.Log("La colisión no fue con el jugador activo.");
            }
        }
        else
        {
            Debug.Log("Cooldown de daño no ha terminado.");
        }
    }
}
