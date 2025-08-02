using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _isGameOver = false;

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

    public void GameOver()
    {

        if (_isGameOver) return;

        _isGameOver = true;
        Debug.Log("Game Over!");

        Time.timeScale = 0f; // Oyunu durcak

        // Burada Game Over UI çaðrýsý yapýlabilir
        // UIManager.Instance.ShowGameOverPanel();
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
