using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad del movimiento
    public float moveDistance = 5f; // Distancia que se moverá en cada dirección

    private Vector2[] waypoints; // Array para los puntos de movimiento
    private int currentWaypointIndex = 0; // Índice del waypoint actual

    private Rigidbody2D playerRb;
    private Vector3 previousPosition;

    void Start()
    {
        // Definir los puntos de movimiento en un cuadrado
        waypoints = new Vector2[4];
        waypoints[0] = transform.position; // Posición inicial
        waypoints[1] = (Vector2)transform.position + Vector2.right * moveDistance; // Derecha
        waypoints[2] = (Vector2)transform.position + Vector2.right * moveDistance + Vector2.up * moveDistance; // Arriba
        waypoints[3] = (Vector2)transform.position + Vector2.up * moveDistance; // Izquierda

        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Mover la plataforma hacia el waypoint actual
        Vector2 targetPosition = waypoints[currentWaypointIndex];
        float step = moveSpeed * Time.fixedDeltaTime;

        // Mover la plataforma hacia la posición objetivo
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        // Si el jugador está sobre la plataforma, aplica la misma velocidad
        if (playerRb != null)
        {
            Vector3 platformMovement = transform.position - previousPosition;
            playerRb.linearVelocity = new Vector2(platformMovement.x / Time.fixedDeltaTime, playerRb.linearVelocity.y);
        }

        previousPosition = transform.position;

        // Verificar si ha llegado al waypoint
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Avanzar al siguiente waypoint
            currentWaypointIndex++;

            // Reiniciar si ha llegado al final del cuadrado
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Reiniciar a la primera posición
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hacer que el jugador se mueva con la plataforma sin anclarlo
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Desacoplar al jugador de la plataforma al salir
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}