using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using JetBrains.Annotations;

public class LeaderboardFetcher : MonoBehaviour
{
    public GameObject scoreEntryPrefab;  // UI prefab (içinde Text veya TMP_Text olacak)
    public Transform contentParent;      // ScrollView -> Viewport -> Content objesi
    public GameObject leaderboardPanel;

    [SerializeField] private GameObject scoreSendingPanel;

    public GameObject userText;
    void Start()
    {
        //FetchLeaderboard();
        userText.GetComponent<TMP_Text>().text = GameSession.CurrentUser;
    }

    public void FetchLeaderboard()
    {
        scoreSendingPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        DatabaseReference scoresRef = FirebaseDatabase.DefaultInstance.GetReference("scores");

        scoresRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogWarning("Firebase skorları alınamadı.");
                return;
            }

            DataSnapshot snapshot = task.Result;

            // Liste oluştur
            List<(string nickname, double score)> entries = new List<(string, double)>();

            foreach (var child in snapshot.Children)
            {
                string nickname = child.Key;
                double.TryParse(child.Value.ToString(), out double score);
                entries.Add((nickname, score));
            }

            // Skorları azalan sıraya göre sırala
            var sortedEntries = entries.OrderByDescending(entry => entry.score).ToList();

            // Önce var olan UI'ları temizle
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            int i = 0;
            // Listeyi UI'da göster
            foreach (var entry in sortedEntries)
            {
                GameObject entryGO = Instantiate(scoreEntryPrefab, contentParent);
                if (entry.nickname.Equals(GameSession.CurrentUser))
                {
                    entryGO.GetComponent<Image>().color = Color.magenta;
                }

                // TMP_Text bileşenlerine erişim
                TMP_Text orderText = entryGO.transform.Find("Order").GetComponent<TMP_Text>();
                TMP_Text nicknameText = entryGO.transform.Find("Nickname").GetComponent<TMP_Text>();
                TMP_Text scoreText = entryGO.transform.Find("Best Score").GetComponent<TMP_Text>();

                orderText.text = (i++ + 1).ToString() + ". ";
                nicknameText.text = entry.nickname;
                scoreText.text = entry.score.ToString();
            }
        });
    }

    public void quitPanel()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        leaderboardPanel.SetActive(false);
        scoreSendingPanel.SetActive(true);
    }

}
