using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour
{
    [SerializeField] private GameObject nicknameObject;
    [SerializeField] private GameObject loginButtonObject;
    [SerializeField] private GameObject messageTextObject;

    [NonSerialized] public TMP_InputField nicknameInput;
    [NonSerialized] public Button loginButton;
    [NonSerialized] public TMP_Text messageText;

    [SerializeField] private FirebaseInit firebaseInit;
    private void Awake()
    {
        nicknameInput = nicknameObject.GetComponent<TMP_InputField>();
        loginButton = loginButtonObject.GetComponent<Button>();
        messageText = messageTextObject.GetComponent<TMP_Text>();
    }
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    void OnLoginClicked()
    {
        string nick = nicknameInput.text.Trim();

        firebaseInit.TryLogin(nick, (success, message) =>
        {
            messageText.text = message;

            if (success)
            {
                GameSession.CurrentUser = nick;
                int sceneIndx = SceneManager.GetActiveScene().buildIndex;
                // Giriş başarılıysa oyun sahnesine geç
                SceneManager.LoadScene(sceneIndx + 1);
            }
        });
    }
}
