using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    [SerializeField] private GameObject _powerDronePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-up alýndý!");
            Transform oldDrone = other.transform;

            // Güçlü drone oluþtur
            GameObject newDrone = Instantiate(_powerDronePrefab, oldDrone.position, oldDrone.rotation);

            // Eski drone'u devre dýþý býrak
            oldDrone.gameObject.SetActive(false);

            // Güçlü drone'a ilerleme hýzýný veya component'leri aktarmak istersem burda yapcam

            Destroy(gameObject);
        }
    }
}
