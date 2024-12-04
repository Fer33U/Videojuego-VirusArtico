using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad del jugador
    private Rigidbody2D playerRb;             // Referencia al Rigidbody2D
    private Vector2 moveInput;                // Entrada de movimiento
    private Animator playerAnimator;

    // Start es llamado antes de la primera actualización
    void Start()
    {
        // Obtener el componente Rigidbody2D del jugador
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        // Comprobar si el componente Rigidbody2D está presente
        if (playerRb == null)
        {
            Debug.LogError("No se encontró un Rigidbody2D en el objeto.");
        }
    }

    // Update es llamado una vez por frame
    void Update()
    {
        // Obtener entrada de movimiento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized; 
        playerAnimator.SetFloat("Horizontal",moveX);
        playerAnimator.SetFloat("Vertical",moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
        // Normalizar el vector para movimientos diagonales consistentes
    }

    // FixedUpdate es llamado en intervalos fijos para manejar la física
    private void FixedUpdate()
    {
        // Mover el Rigidbody2D a la nueva posición
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        }
    }
}
