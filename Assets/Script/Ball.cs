using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float speedIncreaseInterval = 5f;
    public float speedMultiplier = 1.5f;

    public AudioClip hitSound;
    public AudioClip scoreSound; // Nuevo sonido de puntuación

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;

        // Busca el AudioSource dentro de los hijos de la pelota
        audioSource = GetComponentInChildren<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("⚠️ No se encontró un AudioSource en los hijos de la pelota.");
        }

        RestartBall();
        StartCoroutine(IncreaseSpeedOverTime());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                gameManager.ActivatePowerUp(powerUp.powerUpType, this);
                Destroy(collision.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GoalP1"))
        {
            gameManager.AddPoint(2);
            PlayScoreSound();
            RestartBall();
        }
        else if (collision.gameObject.CompareTag("GoalP2"))
        {
            gameManager.AddPoint(1);
            PlayScoreSound();
            RestartBall();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayHitSound();
        }
    }

    void RestartBall()
    {
        transform.position = Vector2.zero;
        currentSpeed = initialSpeed;
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.linearVelocity = randomDirection * currentSpeed;
    }

    public void IncreaseSpeed()
    {
        rb.linearVelocity *= 1.2f;
    }

    public void ReduceSpeed()
    {
        rb.linearVelocity *= 0.8f;
    }

    IEnumerator IncreaseSpeedOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            currentSpeed *= speedMultiplier;
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
    }

    void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
            Debug.Log("🔊 Sonido de rebote reproducido.");
        }
        else
        {
            Debug.LogWarning("⚠️ No se puede reproducir el sonido. Verifica que el AudioSource y el AudioClip estén asignados.");
        }
    }

    void PlayScoreSound()
    {
        if (audioSource != null && scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
            Debug.Log("🏆 Sonido de puntuación reproducido.");
        }
        else
        {
            Debug.LogWarning("⚠️ No se puede reproducir el sonido de puntuación. Verifica que el AudioSource y el AudioClip estén asignados.");
        }
    }
}
