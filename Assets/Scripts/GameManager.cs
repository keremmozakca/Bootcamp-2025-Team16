using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _isGameOver = false;
    private int _score = 0;
    private float _timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (_isGameOver) return;

        _timer += Time.deltaTime;
        if (_timer >= 1f)
        {
            _score++;
            _timer = 0f;
            Debug.Log("Score: " + _score);
        }
    }

    public void GameOver()
    {
        if (_isGameOver) return;

        _isGameOver = true;
        Time.timeScale = 0f; // Oyunu durdur
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void RestartGame()
    {
        _score = 0;
        _timer = 0f;
        _isGameOver = false;
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public int GetScore()
    {
        return _score;
    }
}
