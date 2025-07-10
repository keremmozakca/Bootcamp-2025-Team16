using UnityEngine;
using UnityEngine.SceneManagement;

public class Functions : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(_sceneIndex);
    }
}
