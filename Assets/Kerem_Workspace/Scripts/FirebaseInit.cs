using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FirebaseInit : MonoBehaviour
{ 
    private DatabaseReference activeUsersRef;
    private DatabaseReference scoresRef;

    public string currentNickname = GameSession.CurrentUser;

    public static FirebaseInit Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince silinmesin
        }
        else
        {
            Destroy(gameObject); // Zaten varsa, tekrar oluşturulmasın
        }
    }
    private void OnDestroy()
    {
        Debug.Log("FirebaseInit siliniyor");
        Logout();
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result == DependencyStatus.Available)
            {
                Debug.Log("Firebase başarıyla bağlandı.");
                
                FirebaseApp app = FirebaseApp.DefaultInstance; // Uygulama hazır
                FirebaseApp.DefaultInstance.Options.DatabaseUrl =
new System.Uri("https://yzta-bootcamp-8abbc-default-rtdb.europe-west1.firebasedatabase.app/");

                activeUsersRef = FirebaseDatabase.DefaultInstance.GetReference("activeUsers");
                scoresRef = FirebaseDatabase.DefaultInstance.GetReference("scores");
                //WriteTestData();
            }
            else
            {
                Debug.LogError("Firebase bağlantı hatası: " + result.ToString());
            }
        });
    }

    // Kullanıcı giriş denemesi
    public void TryLogin(string nickname, Action<bool, string> callback)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            callback(false, "Nickname boş olamaz.");
            return;
        }

        activeUsersRef.Child(nickname).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                callback(false, "Firebase bağlantı hatası.");
                Debug.LogError(task.Exception);
                return;
            }

            if (task.Result.Exists)
            {
                // Nickname kullanımda
                callback(false, "Bu nickname kullanımda, lütfen başka bir tane seçin.");
            }
            else
            {
                // Boş, kaydet
                activeUsersRef.Child(nickname).SetValueAsync(true).ContinueWithOnMainThread(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        callback(false, "Kullanıcı aktif olarak kaydedilemedi.");
                        Debug.LogError(t.Exception);
                    }
                    else
                    {
                        currentNickname = nickname;
                        GameSession.CurrentUser = nickname;
                        callback(true, "Giriş başarılı!");
                    }
                });
            }
        });
    }

    // Kullanıcı çıkışı
    public void Logout()
    {
        string nickname = GameSession.CurrentUser;
        if (string.IsNullOrEmpty(currentNickname))
            return;

        activeUsersRef.Child(currentNickname).RemoveValueAsync();
        scoresRef.Child(currentNickname).RemoveValueAsync();
        currentNickname = null;
        GameSession.CurrentUser = null;

        Debug.Log("Çıkış yapıldı.");
    }
    void WriteTestData()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("deneme").SetValueAsync("Merhaba Firebase!").ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Veri yazma başarısız: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Veri başarıyla yazıldı!");
            }
        });
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit çağrıldı");
        Logout();
    }
}
