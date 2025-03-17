using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;

    public Transform player1;
    public Transform player2;

    public GameObject winCanvas;
    public TextMeshProUGUI winText;
    public GameObject ballPrefab;

    public AudioSource winSound; // Sonido de victoria

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private int maxScore = 10;

    private Ball originalBall; // Referencia a la pelota original

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        originalBall = FindObjectOfType<Ball>(); // Guardar la pelota original al iniciar
    }

    public void AddPoint(int player)
    {
        if (player == 1)
        {
            scoreP1++;
            scoreTextP1.text = scoreP1.ToString();
        }
        else if (player == 2)
        {
            scoreP2++;
            scoreTextP2.text = scoreP2.ToString();
        }

        // Eliminar los clones cuando se anote un punto
        RemoveExtraBalls();

        if (scoreP1 >= maxScore)
        {
            ShowWinner(1);
        }
        else if (scoreP2 >= maxScore)
        {
            ShowWinner(2);
        }
    }

    void ShowWinner(int winner)
    {
        winCanvas.SetActive(true);
        winText.text = "¡¡Ha ganado el " + winner + "!!";

        // Detener todos los sonidos en la escena
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // Reproducir el sonido de victoria
        if (winSound != null)
        {
            winSound.Play();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnExtraBall()
    {
        GameObject newBall = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        newBall.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 5f;
    }

    public void ActivatePowerUp(PowerUp.PowerUpType type, Ball ball)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.ShrinkOpponent:
                ShrinkOpponent(ball);
                Debug.Log("Power-Up Activado: Reducir tamaño del oponente");
                break;
            case PowerUp.PowerUpType.GrowSelf:
                GrowSelf(ball);
                Debug.Log("Power-Up Activado: Aumentar tamaño propio");
                break;
            case PowerUp.PowerUpType.SpeedUp:
                ball.IncreaseSpeed();
                Debug.Log("Power-Up Activado: Aumentar velocidad de la pelota");
                break;
            case PowerUp.PowerUpType.SlowOpponent:
                SlowOpponent(ball);
                Debug.Log("Power-Up Activado: Reducir velocidad del oponente");
                break;
            case PowerUp.PowerUpType.MultiBall:
                SpawnExtraBall();
                Debug.Log("Power-Up Activado: Se ha añadido una bola extra");
                break;
        }
    }

    void ShrinkOpponent(Ball ball)
    {
        Transform opponent = (ball.transform.position.x > 0) ? player1 : player2;
        opponent.localScale *= 0.8f;
    }

    void GrowSelf(Ball ball)
    {
        Transform player = (ball.transform.position.x > 0) ? player2 : player1;
        player.localScale *= 1.2f;
    }

    void SlowOpponent(Ball ball)
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (Ball b in balls)
        {
            if (b != ball)
            {
                b.ReduceSpeed();
            }
        }
    }

    void RemoveExtraBalls()
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (Ball ball in balls)
        {
            if (ball != originalBall)
            {
                Destroy(ball.gameObject);
            }
        }
    }
}
