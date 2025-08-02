using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle") || other.CompareTag("Bullet"))
        {
            // Çarpýþan obje ve player yok ediliyor
            Destroy(other.gameObject);
            Destroy(gameObject);
            if(GameManager.Instance == null)
            {
                Debug.LogError("GameManager instance is null!");
                return;
            }
            GameManager.Instance.GameOver(); // Oyun bitirme fonksiyonu çaðrýlýyor

        }
    }
}
