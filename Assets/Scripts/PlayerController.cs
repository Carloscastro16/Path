using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f; // Fuerza del salto
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private bool isGrounded; // Para verificar si el jugador está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Obtener el movimiento del jugador
        float moveX = Input.GetAxisRaw("Horizontal");
        movementInput = new Vector2(moveX, 0).normalized;

        // Comprobar si el jugador presiona la tecla de salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Si hay entrada de movimiento, intentamos mover al jugador
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
                direction,
                movementFilter,
                new List<RaycastHit2D>(),
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isGrounded = false; // Al saltar, el jugador ya no está en el suelo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el jugador está colisionando con el suelo
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true; // El jugador está en el suelo
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica si el jugador sale de la plataforma
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = false; // El jugador no está en el suelo
        }
    }
}