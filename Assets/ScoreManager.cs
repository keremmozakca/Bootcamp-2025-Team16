using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameManager gameManager;

    void Update()
    {
        scoreText.text = "Score: " + gameManager._score;
    }
}
