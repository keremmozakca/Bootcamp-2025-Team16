using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            // �arp��an obje ve player yok ediliyor
            Destroy(other.gameObject);
            Destroy(gameObject);

            // �stersen burada game over ekran� tetikleyebilirsin
            Debug.Log("�arp��ma! Player yok oldu.");
        }
    }
}
