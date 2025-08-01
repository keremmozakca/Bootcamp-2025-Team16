using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    [SerializeField] private GameObject _powerDronePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-up al�nd�!");
            Transform oldDrone = other.transform;

            // G��l� drone olu�tur
            GameObject newDrone = Instantiate(_powerDronePrefab, oldDrone.position, oldDrone.rotation);

            // Eski drone'u devre d��� b�rak
            oldDrone.gameObject.SetActive(false);

            // G��l� drone'a ilerleme h�z�n� veya component'leri aktarmak istersem burda yapcam

            Destroy(gameObject);
        }
    }
}
