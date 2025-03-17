using UnityEngine;

public class Pala : MonoBehaviour
{
    public enum PlayerType { Player1, Player2, AI } 
    public PlayerType player;

    public float speed = 5f;
    public float upperLimit = 4f;
    public float lowerLimit = -4f;
    public Transform ball; // Referencia a la pelota (solo si es IA)

    private Vector2 movement;

    void Update()
    {
        float moveInput = 0f;

        switch (player)
        {
            case PlayerType.Player1:
                moveInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
                break;
            case PlayerType.Player2:
                moveInput = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
                break;
            case PlayerType.AI:
                if (ball != null)
                {
                    // Movimiento simple para seguir la pelota
                    if (ball.position.y > transform.position.y + 0.2f)
                        moveInput = 1;
                    else if (ball.position.y < transform.position.y - 0.2f)
                        moveInput = -1;
                }
                break;
        }

        // Movimiento de la pala
        movement = new Vector2(0, moveInput * speed * Time.deltaTime);
        transform.position += (Vector3)movement;

        // Limitar movimiento dentro de la pantalla
        float clampedY = Mathf.Clamp(transform.position.y, lowerLimit, upperLimit);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    public void ChangeSize(float amount)
    {
        Vector3 newScale = transform.localScale;
        newScale.y = Mathf.Clamp(newScale.y + amount, 0.5f, 2f);
        transform.localScale = newScale;
    }

    public void SlowDown()
    {
        speed *= 0.7f;
        Invoke("ResetSpeed", 5f);
    }

    void ResetSpeed()
    {
        speed = 5f;
    }
}
