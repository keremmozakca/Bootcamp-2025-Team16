using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f; // Merminin �mr�

    private void Start()
    {
        // Merminin �mr� doldu�unda kendini yok et
        Destroy(gameObject, _lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Mermi bir nesneye �arpt���nda kendini yok et
        // ilerde buraya kontroller ekleyece�im
        Destroy(gameObject);
    }
}
