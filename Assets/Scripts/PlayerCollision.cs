using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            // Çarpýþan obje ve player yok ediliyor
            Destroy(other.gameObject);
            Destroy(gameObject);

            // Ýstersen burada game over ekraný tetikleyebilirsin
            Debug.Log("Çarpýþma! Player yok oldu.");
        }
    }
}
