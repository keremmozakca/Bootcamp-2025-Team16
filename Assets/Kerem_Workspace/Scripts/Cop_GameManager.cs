using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cop_GameManager : MonoBehaviour
{
    public static Cop_GameManager Instance { get; private set; }

    private bool _isGameOver = false;
    public int _score = 0;
    private float _timer = 0f;

    public GameObject score;

    private void Start()
    {
        Time.timeScale = 1f;
    }
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
        //score.GetComponent<TMP_Text>().text = "Score: " + _score.ToString();
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
