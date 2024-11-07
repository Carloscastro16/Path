using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public Transform startPosition; // Referencia a la posición inicial

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            ResetPlayerPosition(other.gameObject);
        }
    }

    private void ResetPlayerPosition(GameObject player)
    {
        Debug.Log("Reiniciando posición del jugador...");
        player.transform.position = startPosition.position; // Restablece la posición del jugador
    }
}