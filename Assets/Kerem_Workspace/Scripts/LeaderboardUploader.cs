using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class LeaderboardUploader : MonoBehaviour
{
    //string currentUser = GameSession.CurrentUser;
    public void UploadScoreIfBest(string nickname, double currentScore)
    {
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference("scores").Child(nickname);
        dbRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogWarning($"[Firebase] Skor kontrolü başarısız: {task.Exception}");
                return;
            }

            double bestScore = 0.0;
            bool hasPreviousScore = false;

            if (task.Result.Exists)
            {
                hasPreviousScore = true;
                double.TryParse(task.Result.Value.ToString(), out bestScore);
            }

            if (!hasPreviousScore || currentScore > bestScore)
            {
                dbRef.SetValueAsync(currentScore).ContinueWithOnMainThread(setTask =>
                {
                    if (setTask.IsCompleted)
                        Debug.Log($"[Firebase] Skor eklendi/güncellendi: {nickname} → {currentScore}");
                    else
                        Debug.LogWarning($"[Firebase] Skor güncelleme hatası: {setTask.Exception}");
                });
            }
            else
            {
                Debug.Log($"[Firebase] Skor yeterince yüksek değil, güncellenmedi. (Eski: {bestScore}, Yeni: {currentScore})");
            }
        });
    }

    private void Start()
    {
        UploadScoreIfBest(GameSession.CurrentUser, 0);
    }
}
